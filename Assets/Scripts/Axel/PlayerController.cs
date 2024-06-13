using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ParticleSystem dashTrail;

    [Space]

    [SerializeField] Color damagedColor;

    [Space]

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalView;
    [HideInInspector] public float verticalView;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public int canDash = 0;

    [HideInInspector] public Health _playerHealth;
    [HideInInspector] public Animator _playerAnimator;
    [HideInInspector] public EnemiesManager currentEnemiesManager;
    [HideInInspector] public Rigidbody2D _playerRigidbody;

    Vector2 moveDir;

    SpriteRenderer _playerSpriteRenderer;

    public VolumeProfile volumeProfile;
    Vignette vignette;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        verticalInput = -1;
        verticalView = -1;

        Vignette tmp;
        if (volumeProfile.TryGet(out tmp)) vignette = tmp;
    }

    private void Update()
    {
        if (!canMove) return;

        PlayerInput();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        PlayerMovement();
    }

    void PlayerInput()
    {
        horizontalInput = UserInput.instance.moveInput.x;
        verticalInput = UserInput.instance.moveInput.y;

        horizontalInput = Mathf.Abs(horizontalInput) <= .3f ? 0 : horizontalInput;
        verticalInput = Mathf.Abs(verticalInput) <= .3f ? 0 : verticalInput;

        if (horizontalInput != 0)
        {
            horizontalView = horizontalInput;
            verticalView = 0;
        }
        if (verticalInput != 0)
        {
            verticalView = verticalInput;
            horizontalView = 0;
        }

        if(horizontalInput != 0 && verticalInput != 0)
        {
            horizontalInput /= 1.25f;
            verticalInput /= 1.25f;
        }

        _playerAnimator.SetFloat("Horizontal", horizontalInput);
        _playerAnimator.SetFloat("Vertical", verticalInput);

        _playerAnimator.SetFloat("HorizontalView", horizontalView);
        _playerAnimator.SetFloat("VerticalView", verticalView);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _playerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            _playerAnimator.SetBool("IsRunning", false);
        }

        if (UserInput.instance.dashInput && canDash == 0)
        {
            canDash = 2;

            dashTrail.Play();

            Invoke(nameof(DashReset), StatsManager.instance.dashCooldown);
            Invoke(nameof(MoveReset), StatsManager.instance.dashCooldown / 2.5f);
            HudManager.instance.StartCoroutine(HudManager.instance.DashCooldownBar(StatsManager.instance.dashCooldown));
        }
    }

    void PlayerMovement()
    {
        moveDir = new Vector2(horizontalInput, verticalInput);

        _playerRigidbody.AddForce(moveDir * StatsManager.instance.speed, ForceMode2D.Force);

        if (canDash == 2)
        {
            _playerRigidbody.velocity = Vector2.zero;

            canDash = 1;

            Vector2 viewDir = new Vector2(horizontalView == 0 ? horizontalInput : horizontalView, verticalView == 0 ? verticalInput : verticalView);

            _playerRigidbody.AddForce(viewDir * StatsManager.instance.dashSpeed, ForceMode2D.Impulse);

            canMove = false;
        }
    }

    public void DamagedIFrames(float duration, bool knockBack, Vector2 dir)
    {
        canTakeDamage = false;
        Invoke(nameof(DamageReset), duration);

        StartCoroutine(DamagedAnimation(duration));

        if (knockBack)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce((dir != null ? dir : Vector2.zero) * 1200, ForceMode2D.Impulse);
        }
    }

    public IEnumerator DamagedAnimation(float duration)
    {
        Color vignetteStartColor = vignette.color.value;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            if (currentTime < duration / 2f)
            {
                vignette.color.value = Color.Lerp(vignette.color.value, damagedColor, Mathf.PingPong(Time.time, currentTime));
                _playerSpriteRenderer.color = Color.Lerp(_playerSpriteRenderer.color, damagedColor, Mathf.PingPong(Time.time, currentTime));
            }
            else
            {
                vignette.color.value = Color.Lerp(vignette.color.value, vignetteStartColor, Mathf.PingPong(Time.time, currentTime / 2));
                _playerSpriteRenderer.color = Color.Lerp(_playerSpriteRenderer.color, Color.white, Mathf.PingPong(Time.time, currentTime / 2));
            }

            yield return null;
        }

        vignette.color.value = vignetteStartColor;
        _playerSpriteRenderer.color = Color.white;
    }

    void DashReset()
    {
        canDash = 0;
    }

    void MoveReset()
    {
        canMove = true;
    }

    public void MoveUpdate(bool active)
    {
        canMove = active;
        _playerAnimator.SetBool("IsRunning", active);
        horizontalInput = 0;
        verticalInput = 0;
    }

    public void DamageReset()
    {
        canTakeDamage = true;
    }

    public void Death()
    {
        StopAllCoroutines();
        canMove = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 15.00f;

    [Space]

    public float dashSpeed = 15.00f;
    public float dashCooldown = 1f;
    [SerializeField] ParticleSystem dashTrail;

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

    Vector2 moveDir;
    Vector2 startVelocity;

    Rigidbody2D _playerRigidbody;
    SpriteRenderer _playerSpriteRenderer;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

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

        if (Input.GetKeyDown(KeyCode.Space) && canDash == 0)
        {
            canDash = 2;
            canTakeDamage = false;

            startVelocity = _playerRigidbody.velocity;

            dashTrail.Play();

            Invoke(nameof(DashReset), dashCooldown);
            Invoke(nameof(DamageReset), dashCooldown / 3);
            StartCoroutine(HudManager.instance.DashCooldownBar(dashCooldown));
        }

        // Dev tool
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void PlayerMovement()
    {
        moveDir = new Vector2(horizontalInput, verticalInput);

        if (canMove)
        {
            _playerRigidbody.AddForce(moveDir * speed, ForceMode2D.Force);

            if (canDash == 2)
            {
                canDash = 1;

                Vector2 viewDir = new Vector2(horizontalView == 0 ? horizontalInput : horizontalView, verticalView == 0 ? verticalInput : verticalView);

                _playerRigidbody.AddForce(viewDir * dashSpeed, ForceMode2D.Impulse);
            }
        }
    }

    public IEnumerator IFrameAnimation(float duration, int colorShiftTimes, bool knockBack, Vector2 dir)
    {
        _playerSpriteRenderer.color = Color.red;

        if (knockBack)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce((dir != null ? dir : Vector2.zero) * 1000, ForceMode2D.Impulse);
        }

        for (int i = 0; i < colorShiftTimes; i++)
        {
            yield return new WaitForSeconds(duration / colorShiftTimes);
            _playerSpriteRenderer.color = _playerSpriteRenderer.color == Color.white ? Color.red : Color.white;
        }

        _playerSpriteRenderer.color = Color.white;
    }

    void DashReset()
    {
        canDash = 0;
    }

    public void DamageReset()
    {
        canTakeDamage = true;
    }

    public void Death()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}

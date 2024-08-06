using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;
using System.Reflection;

public class PlayerController : Entity
{
    public static PlayerController instance;

    public float healthMultiplier;
    public float damageMultiplier;
    public float speedMultiplier;
    public float defenseMultiplier;
    public float attackSpeedMultiplier;

    [Space]

    [SerializeField] float dashCooldown;
    [SerializeField] float dashSpeed;

    [Space]

    [SerializeField] ParticleSystem dashTrail;
    public Color damagedColor;

    int canDash = 0;

    float horizontalInput;
    float verticalInput;
    float horizontalView;
    float verticalView;

    Vector2 moveDir;

    Animator _playerAnimator;

    [HideInInspector] public Weapon currentWeapon;
    [HideInInspector] public EnemiesManager currentEnemiesManager;
    [HideInInspector] public Rigidbody2D _playerRigidbody;
    [HideInInspector] public SpriteRenderer _playerSpriteRenderer;

    public FieldInfo[] properties = typeof(PlayerController).GetFields();

    public override void Awake()
    {
        base.Awake();

        instance = this;

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();

        verticalInput = -1;
        verticalView = -1;
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

        if (horizontalInput != 0 && verticalInput != 0)
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

            Invoke(nameof(DashReset), dashCooldown);
            Invoke(nameof(MoveReset), .16f);
            HudManager.instance.StartCoroutine(HudManager.instance.DashCooldownBar(dashCooldown));
        }
    }

    void PlayerMovement()
    {
        moveDir = new Vector2(horizontalInput, verticalInput);

        _playerRigidbody.AddForce(moveDir * speedCurrent * 100, ForceMode2D.Force);

        if (canDash == 2)
        {
            _playerRigidbody.velocity = Vector2.zero;

            canDash = 1;

            Vector2 viewDir = new Vector2(horizontalView == 0 ? horizontalInput : horizontalView, verticalView == 0 ? verticalInput : verticalView);

            _playerRigidbody.AddForce(viewDir * dashSpeed * 100, ForceMode2D.Impulse);

            canMove = false;
        }
    }

    public void OnHit(float iFramesDuration, bool knockBack, Vector2 dir, Entity hitingEntity)
    {
        if (healthCurrent <= 0) return;

        canTakeDamage = false;

        Invoke(nameof(DamageReset), iFramesDuration);
        StartCoroutine(DamagedAnimation(iFramesDuration));

        if (knockBack)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce((dir != null && Vector3.Distance(transform.position, hitingEntity.transform.position) > .1f ? 
                dir : Vector2.zero) * 1200, ForceMode2D.Impulse);
        }

        if (hitingEntity.effectsManager.appliesEffects)
        {
            effectsManager.ApplyEffect(hitingEntity.effectsManager);
        }
    }

    public IEnumerator DamagedAnimation(float duration)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            if (currentTime < duration / 2f)
            {
                _playerSpriteRenderer.color = Color.Lerp(_playerSpriteRenderer.color, damagedColor, Mathf.PingPong(Time.time, currentTime));
            }
            else
            {
                _playerSpriteRenderer.color = Color.Lerp(_playerSpriteRenderer.color, Color.white, Mathf.PingPong(Time.time, currentTime / 2));
            }

            yield return null;
        }

        _playerSpriteRenderer.color = Color.white;
    }

    public void SetStat(Stat stat, float statChange)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat));
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Current");

        float changedValue = (float)property.GetValue(this) + statChange;
        float currentChangedValue = (float)currentProperty.GetValue(this) + statChange;

        property.SetValue(this, changedValue);
        currentProperty.SetValue(this, currentChangedValue);
    }
    public float GetStat(Stat stat)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat));

        return (float)property.GetValue(this);
    }

    public float GetCurrentStat(Stat stat)
    {
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Current");

        return (float)currentProperty.GetValue(this);
    }

    public float GetStatMultiplier(Stat stat)
    {
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Multiplier");

        return (float)currentProperty.GetValue(this);
    }

    string GetStatName(Stat stat)
    {
        return stat.ToString();
    }
    void DashReset()
    {
        canDash = 0;
    }

    void MoveReset()
    {
        canMove = true;
    }

    void DamageReset()
    {
        canTakeDamage = true;
    }

}

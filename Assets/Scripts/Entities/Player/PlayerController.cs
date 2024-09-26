using System.Collections;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : Entity, IDataPersistence
{
    public static PlayerController instance;

    [Space]

    public float healthStartValue;
    public float damageStartValue;
    public float defenseStartValue;
    public float speedStartValue;
    public float attackSpeedStartValue;

    [Space]

    public float healthMultiplier;
    public float damageMultiplier;
    public float speedMultiplier;
    public float defenseMultiplier;
    public float attackSpeedMultiplier;

    [Space]

    public MinMaxStat healthMinMax;
    public MinMaxStat damageMinMax;
    public MinMaxStat defenseMinMax;
    public MinMaxStat speedMinMax;
    public MinMaxStat attackSpeedMinMax;

    [Serializable]
    public struct MinMaxStat
    {
        public float min;
        public float max;
    }

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

    [HideInInspector] public string lastHitByName;

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

    public override void Update()
    {
        base.Update();

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

            Audio.instance.PlayOneShot(Audio.Sound.swoosh, .15f);

            dashTrail.Play();

            Invoke(nameof(DashReset), dashCooldown);
            Invoke(nameof(MoveReset), .16f);
            HudManager.instance.StartCoroutine(HudManager.instance.DashCooldownBar(dashCooldown));
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            RemoveHealth(5f);
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

    public void OnHit(float iFramesDuration, Entity hitingEntity)
    {
        if (healthCurrent <= 0) return;

        canTakeDamage = false;

        Invoke(nameof(DamageReset), iFramesDuration);
        StartCoroutine(DamagedAnimation(iFramesDuration));
        StartCoroutine(JiggleAnimation(10));

        Audio.instance.PlayOneShot(Audio.Sound.hurt, .5f);

        lastHitByName = hitingEntity.entityName;

        if (hitingEntity.effectsManager.appliesEffects)
        {
            effectsManager.ApplyEffect(hitingEntity.effectsManager);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        GameManager.instance.isGameOver = true;

        GameOverMenu.instance.SetGameOverPanel();
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

    public void ResetStats()
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            SetStat(stat, (float)GetStatStartValue(stat).GetValue(this), false);
        }
    }

    public bool CanSetStat(Stat stat) 
    {
        return (Mathf.Abs(GetStat(stat)) < GetMinMaxStat(stat).max && Mathf.Abs(GetStat(stat)) > GetMinMaxStat(stat).min);
    }

    public void SetStat(Stat stat, float statChange, bool addValue)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat));
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Current");

        float changedValue = 0;
        float currentChangedValue = 0;

        if (addValue)
        {
            changedValue = (float)property.GetValue(this) + statChange;
            currentChangedValue = (float)currentProperty.GetValue(this) + statChange;
        }
        else
        {
            changedValue = statChange;
            currentChangedValue = statChange;
        }

        StatCheck(ref changedValue, stat);
        StatCheck(ref currentChangedValue, stat);

        property.SetValue(this, changedValue);
        currentProperty.SetValue(this, currentChangedValue);
    }

    void StatCheck(ref float value, Stat stat)
    {
        if (value >= GetMinMaxStat(stat).max)
        {
            value = GetMinMaxStat(stat).max;
        }
        else if (value <= GetMinMaxStat(stat).min)
        {
            value = GetMinMaxStat(stat).min;
        }
    }

    public float GetStat(Stat stat)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat));

        return (float)property.GetValue(this);
    }

    public MinMaxStat GetMinMaxStat(Stat stat)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat) + "MinMax");

        return (MinMaxStat)property.GetValue(this);
    }

    public FieldInfo GetStatStartValue(Stat stat)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat) + "StartValue");

        return property;
    }

    public float GetCurrentStat(Stat stat)
    {
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Current");

        return Mathf.Clamp((float)currentProperty.GetValue(this), GetMinMaxStat(stat).min, GetMinMaxStat(stat).max);
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

    public void MoveReset()
    {
        canMove = true;
    }

    void DamageReset()
    {
        canTakeDamage = true;
    }

    public void LoadData(GameData data)
    {
        health = data.health;
        healthCurrent = data.healthCurrent;
        defense = data.defense;
        defenseCurrent = data.defenseCurrent;
        damage = data.damage;
        damageCurrent = data.damageCurrent;
        attackSpeed = data.attackSpeed;
        attackSpeedCurrent = data.attackSpeedCurrent;

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name)) transform.position = data.playerPos;
    }

    public void SaveData(GameData data)
    {
        data.health = health;
        data.healthCurrent = healthCurrent;
        data.defense = defense;
        data.defenseCurrent = defenseCurrent;
        data.damage = damage;
        data.damageCurrent = damageCurrent;
        data.attackSpeed = attackSpeed;
        data.attackSpeedCurrent = attackSpeedCurrent;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject[] attackIcons;

    public int attackIndex = 0;

    public bool attack = false;
    public float attackDelay = 1f;

    [Space]

    public int throwAttack = 0;
    [HideInInspector] public int currentAmmo = 6;
    public float throwAttackReloadTime = .5f;
    [SerializeField] GameObject throwingAttack;

    PlayerController playerController;
    DamageManager _damageManager;
    Animator _orbAnimator;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _orbAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _damageManager = GetComponent<DamageManager>();
    }

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();

        _damageManager.damage = StatsManager.instance.stats[1].statValue;

        currentAmmo = (int)StatsManager.instance.stats[4].statValue;
    }

    private void Update()
    {
        UpdateAttackDir();

        if (!playerController.canMove) return;

        PlayerInput();
    }

    void PlayerInput()
    {
        if (UserInput.instance.changeAttackInput)
        {
            UpdateHUD();
        }

        if (UserInput.instance.attackInput)
        {
            isAttacking = true;

            if (!attack && attackIndex == 0)
            {
                attack = true;
                StartCoroutine(AttackReset());

                _orbAnimator.SetTrigger("Attack");
            }

            if(throwAttack == 0 && attackIndex == 1)
            {
                if(currentAmmo > 0)
                {
                    HudManager.instance.UpdateAmmoBar(currentAmmo);
                    currentAmmo--;

                    throwAttack = 2;
                    Invoke(nameof(ThrowAttackReset), StatsManager.instance.stats[3].statValue);

                    DamageManager throwDamage = Instantiate(throwingAttack, transform.position, transform.rotation).GetComponent<DamageManager>();
                    throwDamage.damage = StatsManager.instance.stats[2].statValue;

                    if(currentAmmo <= 0)
                    {
                        UpdateHUD();
                    }
                }
            }
        }
        else if(UserInput.instance.attackInputUp)
        {
            isAttacking = false;
        }
    }
    bool isAttacking;

    float horizontalInput;
    float verticalInput = -1;

    float horizontalView = 0;
    float verticalView = -1;

    bool lockOnEnemy = false;

    [SerializeField] GameObject lockEnemyDisplay;

    void UpdateAttackDir()
    {
        horizontalInput = UserInput.instance.viewInput.x;
        verticalInput = UserInput.instance.viewInput.y;

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

        Vector3 pos = Vector2.down;

        EnemiesManager currentEnemiesManager = playerController.currentEnemiesManager;
        EnemyController lockedEnemy = currentEnemiesManager != null ? currentEnemiesManager.GetClosestEnemyToPoint(transform.position) : null;

        if (UserInput.instance.lockEnemyInput)
        {
            lockOnEnemy = lockOnEnemy ? false : true;
        }

        if (currentEnemiesManager != null && lockedEnemy != null && lockOnEnemy)
        {
            pos = lockedEnemy.transform.position;

            lockEnemyDisplay.SetActive(true);
            lockEnemyDisplay.transform.position = pos;
        }
        else 
        {
            lockEnemyDisplay.SetActive(false);

            if (horizontalInput != 0 || verticalInput != 0)
            {
                pos = transform.position + new Vector3(horizontalInput, verticalInput);
            }
            else
            {
                pos = transform.position + new Vector3(horizontalView, verticalView);
            }
        }

        Vector3 direction = pos - transform.position;
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;

        if (!isAttacking && !attack) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * 30);
    }

    public void UpdateHUD()
    {
        if (currentAmmo > 0)
        {
            if (attackIndex < 1)
            {
                attackIndex++;
            }
            else
            {
                attackIndex = 0;
            }
        }
        else
        {
            attackIndex = 0;
        }

        for (int i = 0; i < attackIcons.Length; i++)
        {
            attackIcons[i].SetActive(i == attackIndex);
        }

        HudManager.instance.ammoBar.SetActive(attackIndex == 1);
    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(attackDelay);

        attack = false;

        transform.localScale = new Vector3(transform.localScale.x == 1 ? -1 : 1, transform.localScale.y, transform.localScale.z);
    }

    public void ThrowAttackReset()
    {
        throwAttack = 0;
    }
}

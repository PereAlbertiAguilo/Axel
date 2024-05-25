using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool attack = false;
    public float attackDelay = 1f;

    [Space]

    public int throwAttack = 0;
    public int maxAmmo = 6;
    [HideInInspector] public int currentAmmo = 6;
    public float throwAttackDelay = 1f;
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

        HudManager.instance.FillAmmoBar(maxAmmo);
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;

        if (Input.GetMouseButton(0) && !attack)
        {
            attack = true;
            StartCoroutine(AttackReset());

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            playerController.speed /= 2f;
            playerController._playerAnimator.speed /= 2f;

            _orbAnimator.SetTrigger("Attack");
        }

        if (Input.GetMouseButton(1) && throwAttack == 0 && currentAmmo > 0)
        {
            HudManager.instance.UpdateAmmoBar(currentAmmo);
            currentAmmo--;

            throwAttack = 2;
            Invoke(nameof(ThrowAttackReset), throwAttackDelay);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Instantiate(throwingAttack, transform.position, transform.rotation);
        }

        if (!attack)
        {
            _orbAnimator.SetFloat("Horizontal", playerController.horizontalView);
            _orbAnimator.SetFloat("Vertical", playerController.verticalView);
        }
        else
        {
            if (mousePos.y > transform.position.y)
            {
                _spriteRenderer.sortingOrder = 0;
            }
            else
            {
                _spriteRenderer.sortingOrder = 1;
            }
        }
    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(attackDelay);

        attack = false;

        playerController.speed *= 2f;
        playerController._playerAnimator.speed *= 2f;

        transform.localScale = new Vector3(transform.localScale.x == 1 ? -1 : 1, transform.localScale.y, transform.localScale.z);
    }

    public void ThrowAttackReset()
    {
        throwAttack = 0;
    }
}

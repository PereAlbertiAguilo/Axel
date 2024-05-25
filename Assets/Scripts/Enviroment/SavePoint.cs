using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    PlayerController playerController;
    HudManager hudManager;
    PlayerAttack playerAttack;

    Animator _animator;

    bool isInRange = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        hudManager = FindObjectOfType<HudManager>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerAttack.currentAmmo < playerAttack.maxAmmo && 
            (playerAttack.throwAttack == 0 || playerAttack.throwAttack == 2) && isInRange)
        {
            playerAttack.CancelInvoke();
            playerAttack.throwAttack = 1;
            playerAttack.Invoke(nameof(playerAttack.ThrowAttackReset), playerAttack.throwAttackReloadTime);

            HudManager.instance.StartCoroutine(HudManager.instance.ReloadAmmoBar
                (playerAttack.maxAmmo - playerAttack.currentAmmo, playerAttack.throwAttackReloadTime));
            playerAttack.currentAmmo = playerAttack.maxAmmo;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            _animator.SetBool("IsInRange", isInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            _animator.SetBool("IsInRange", isInRange);
        }
    }
}

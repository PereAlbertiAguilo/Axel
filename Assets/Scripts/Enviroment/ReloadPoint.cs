using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPoint : MonoBehaviour
{
    [SerializeField] float waitTime = 1f;

    PlayerController playerController;
    HudManager hudManager;
    PlayerAttack playerAttack;

    Animator _animator;

    bool canInteract = false;

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
        if (UserInput.instance.interactInput && playerAttack.currentAmmo < (int)StatsManager.instance.stats[4].statValue && 
            (playerAttack.throwAttack == 0 || playerAttack.throwAttack == 2) && canInteract)
        {
            float currentAmmo = playerAttack.currentAmmo;

            playerAttack.CancelInvoke();
            playerAttack.throwAttack = 1;
            playerAttack.Invoke(nameof(playerAttack.ThrowAttackReset), playerAttack.throwAttackReloadTime);

            HudManager.instance.StartCoroutine(HudManager.instance.ReloadAmmoBar
                ((int)StatsManager.instance.stats[4].statValue - playerAttack.currentAmmo, playerAttack.throwAttackReloadTime));
            
            playerAttack.currentAmmo = (int)StatsManager.instance.stats[4].statValue;

            if (currentAmmo <= 0) playerAttack.UpdateHUD();
        }
    }

    IEnumerator InteractDelay(bool enter)
    {
        yield return new WaitForSeconds(waitTime);

        canInteract = enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(InteractDelay(true));
            _animator.SetBool("IsInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            canInteract = false;
            _animator.SetBool("IsInRange", false);
        }
    }
}

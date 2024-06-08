using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollide : MonoBehaviour
{
    DamageManager _damageManager;

    private void Awake()
    {
        _damageManager = GetComponent<DamageManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if(playerController != null && playerController.canTakeDamage)
            {
                Vector2 heading = playerController.transform.position - transform.position;
                Vector2 direction = heading / heading.magnitude;

                playerController.DamagedIFrames(.75f, true, 
                    Vector3.Distance(playerController.transform.position, transform.position) > .1f ? direction : Vector2.zero);
                playerController._playerHealth.RemoveHealth(_damageManager.damage, playerController.Death);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Enemy enemyController))
        {
            enemyController.OnHit();

            enemyController.RemoveHealth(PlayerController.instance.damageCurrent);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemyController))
        {
            enemyController.OnHit();

            enemyController.RemoveHealth(PlayerController.instance.damageCurrent);
        }
    }
}

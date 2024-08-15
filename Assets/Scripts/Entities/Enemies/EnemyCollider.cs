using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController) )
        {
            if(playerController.canTakeDamage && enemy.canDealDamage)
            {
                playerController.RemoveHealth(enemy.damageCurrent);

                playerController.OnHit(.75f, enemy);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            if (playerController.canTakeDamage && enemy.canDealDamage)
            {
                playerController.RemoveHealth(enemy.damageCurrent);

                playerController.OnHit(.75f, enemy);
            }
        }
    }
}

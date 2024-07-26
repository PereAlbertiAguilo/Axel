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
            if(playerController.canTakeDamage)
            {
                playerController.OnHit(.75f, true, Direction.Normalized(PlayerController.instance.transform.position, transform.position), enemy);

                playerController.RemoveHealth(enemy.damageCurrent);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            if (playerController.canTakeDamage)
            {
                playerController.OnHit(.75f, true, Direction.Normalized(PlayerController.instance.transform.position, transform.position), enemy);

                playerController.RemoveHealth(enemy.damageCurrent);
            }
        }
    }
}

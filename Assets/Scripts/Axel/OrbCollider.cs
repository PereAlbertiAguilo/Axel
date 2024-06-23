using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollider : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Health _health;
    EnemyController _enemyController;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _enemyController = GetComponent<EnemyController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator HitColorShift()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.15f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(.15f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.15f);
        _spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Orb"))
        {
            StartCoroutine(HitColorShift());

            DamageManager damageManager = collision.transform.parent.GetComponent<DamageManager>();

            if (_health != null)
            {
                _health.RemoveHealth(damageManager.damage, null);
            }
            if (_enemyController != null)
            {
                _enemyController.OnHit();
            }
        }
    }
}

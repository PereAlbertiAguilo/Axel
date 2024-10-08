using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Enemy enemyController) && PlayerController.instance.canDealDamage)
        {
            Audio.instance.PlayOneShot(Audio.Sound.slash, .3f);

            enemyController.RemoveHealth(PlayerController.instance.damageCurrent + PlayerController.instance.currentWeapon.weaponAddedDamage);

            enemyController.OnHit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemyController) && PlayerController.instance.canDealDamage)
        {
            Audio.instance.PlayOneShot(Audio.Sound.slash, .3f);

            enemyController.RemoveHealth(PlayerController.instance.damageCurrent + PlayerController.instance.currentWeapon.weaponAddedDamage);

            enemyController.OnHit();
        }
    }
}

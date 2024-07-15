using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Weapon
{
    [SerializeField] GameObject projetile;

    public override void Attack()
    {
        UseRangedWeapon();

        base.Attack();
    }

    public void UseRangedWeapon()
    {
        Instantiate(projetile, transform.position, transform.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Weapon
{
    [SerializeField] GameObject projetile;

    private void Awake()
    {
        SetWeapon(UseRangedWeapon);
    }

    public void UseRangedWeapon()
    {
        Instantiate(projetile, transform.position, transform.rotation);
    }
}

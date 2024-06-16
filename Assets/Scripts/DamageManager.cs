using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public bool isPlayer = true;

    public float damage = 1f;

    public void UpdateDamage(float damage)
    {
        this.damage = damage;
    }
}

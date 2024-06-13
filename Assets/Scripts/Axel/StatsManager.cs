using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public float normalDamage;
    public float throwDamage;
    public float throwAttackFireRate;
    public int maxAmmo;
    public float speed;
    public float dashSpeed;
    public float dashCooldown;

    //[Space]

    //public float normalDamageMax;
    //public float throwDamageMax;
    //public float throwAttackFireRateMax;
    //public int maxAmmoMax;
    //public float speedMax;
    //public float dashSpeedMax;
    //public float dashCooldownMax;

    private void Awake()
    {
        instance = this;
    }
}

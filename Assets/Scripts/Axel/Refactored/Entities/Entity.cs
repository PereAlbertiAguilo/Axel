using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Stat
    {
        health, defense, speed, damage, attackSpeed
    };

    [HideInInspector] public float health;
    [HideInInspector] public float healthCurrent;
    [HideInInspector] public float damage;
    [HideInInspector] public float damageCurrent;
    [HideInInspector] public float speed;
    [HideInInspector] public float speedCurrent;
    [HideInInspector] public float defense;
    [HideInInspector] public float defenseCurrent;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float attackSpeedCurrent;
    [HideInInspector] public bool isMobile;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canDealDamage;
    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public EffectsManager effectsManager;

    public virtual void Awake()
    {
        healthCurrent = health;
        speedCurrent = speed;
        attackSpeedCurrent = attackSpeed;
        defenseCurrent = defense;
        damageCurrent = damage;
    }

    public virtual void Start()
    {

    }

    public void AddHealth(float healthToAdd)
    {
        if (healthCurrent < health)
        {
            healthCurrent += healthToAdd;

            PopUp.instance.Message(transform, "" + Math.Round(healthToAdd, 2), Color.green, .3f, true);
        }
        else
        {
            healthCurrent = health;
        }
    }

    public void DealDamage(float healthToRemove)
    {
        if (healthCurrent > 0)
        {
            healthCurrent -= healthToRemove;

            PopUp.instance.Message(transform, "" + Math.Round(healthToRemove, 2), Color.red, .3f, true);

            if (healthCurrent <= 0)
            {
                healthCurrent = 0;

                gameObject.SetActive(false);
            }
        }
    }
}

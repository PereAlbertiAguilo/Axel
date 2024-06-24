using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public DamagePopUp damagePopUp;

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
        damagePopUp = GetComponent<DamagePopUp>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void RemoveHealth(float healthRemoved, Action deathStateFunc)
    {
        if(currentHealth > 0)
        {
            currentHealth -= healthRemoved;

            if(healthBar != null )
            {
                healthBar.StartCoroutine(healthBar.UpdateHealthBar(currentHealth, maxHealth, .2f));
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                if (deathStateFunc != null)
                {
                    deathStateFunc?.Invoke();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        } 
    }

    public void AddHealth(float healthAdded)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healthAdded;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }
}

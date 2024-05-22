using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void RemoveHealth(float healthRemoved, Action deathStateFunc)
    {
        if(currentHealth > 0)
        {
            currentHealth -= healthRemoved;

            HealthBar healthBar = GetComponent<HealthBar>();

            if(healthBar != null )
            {
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
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

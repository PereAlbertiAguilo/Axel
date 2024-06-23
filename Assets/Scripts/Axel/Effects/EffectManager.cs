using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EffectManager : MonoBehaviour
{
    [Range(1, 10)]
    public int effectPower = 1;

    public float duration = 2f;
    float currentTime;

    bool effectOn = false;

    private void Start()
    {
        StatsManager.Stat statSlowness = StatsManager.instance.GetStat(StatsManager.StatType.speed);
        StatsManager.Stat statWeakness = StatsManager.instance.GetStat(StatsManager.StatType.nDamage);
        Health playerHealth = FindObjectOfType<PlayerController>()._playerHealth;

        StartCoroutine(Slowness(statSlowness.statValue, (i) => statSlowness.statValue = i, (i) => statSlowness.statValue = i));
        StartCoroutine(Delay(Dot(playerHealth, 2, (i) => playerHealth.currentHealth = i)));
        StartCoroutine(Weakness(statWeakness.statValue, (i) => statWeakness.statValue = i, (i) => statWeakness.statValue = i));
    }

    private void Update()
    {
        if (effectOn)
        {
            if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                effectOn = false;
                currentTime = duration;
            }
        }
    }

    public IEnumerator Slowness(float startValue, Action<float> changeValue, Action<float> resetValue)
    {
        effectOn = true;

        float currentValue = startValue;
        currentValue -= PowFromValue(startValue);
        changeValue(currentValue);

        yield return new WaitForSeconds(duration);

        resetValue(startValue);
    }

    public IEnumerator Dot(Health health, float dotPower, Action<float> changeValue)
    {
        effectOn = true;

        float currentHealth = health.currentHealth;

        while (effectOn)
        {
            currentHealth -= PowFromValue(dotPower);

            changeValue(currentHealth);

            health.healthBar.StartCoroutine(health.healthBar.UpdateHealthBar(currentHealth, health.maxHealth, .1f));

            yield return new WaitForSeconds(.75f);
        }
    }

    public IEnumerator Weakness(float startValue, Action<float> changeValue, Action<float> resetValue)
    {
        effectOn = true;

        float currentValue = startValue;
        currentValue -= PowFromValue(startValue);
        changeValue(currentValue);

        yield return new WaitForSeconds(duration);

        resetValue(startValue);
    }

    IEnumerator Delay(IEnumerator enumerator) 
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(enumerator);
    }

    float PowFromValue(float value)
    {
        return effectPower * value / 11;
    }
}

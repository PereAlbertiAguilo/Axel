using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EffectManager;

public class EffectManager : MonoBehaviour
{
    public enum EffectType
    {
        slowness, dot, weakness
    }


    [Serializable]
    public class Effect
    {
        public string name;
        [Range(1, 10)]
        public int effectPower = 1;
        public float duration = 2f;
        public float currentTime;
        public EffectType type;
        [HideInInspector] public bool effectActive = false;
    }

    [Space]

    public List<Effect> effects = new List<Effect>(1);

    public void EnemyEffect()
    {
        foreach (Effect effect in effects)
        {
            switch (effect.type)
            {
                case EffectType.slowness:
                    StatsManager.Stat statSlowness = StatsManager.instance.GetStat(StatsManager.StatType.speed);
                    StartCoroutine(Slowness(statSlowness.statValue, (i) => statSlowness.statValue = i, (i) => statSlowness.statValue = i));
                    break;
                case EffectType.dot:
                    Health playerHealth = FindObjectOfType<PlayerController>()._playerHealth;
                    StartCoroutine(Dot(playerHealth));
                    break;
                case EffectType.weakness:
                    StatsManager.Stat statWeakness = StatsManager.instance.GetStat(StatsManager.StatType.nDamage);
                    StartCoroutine(Weakness(statWeakness.statValue, (i) => statWeakness.statValue = i, (i) => statWeakness.statValue = i));
                    break;
            }
        }
    }

    public void PlayerEffect(GameObject enemy)
    {
        foreach (Effect effect in effects)
        {
            switch (effect.type)
            {
                case EffectType.slowness:
                    AIPath aIPath = enemy.GetComponent<AIPath>();
                    StartCoroutine(Slowness(aIPath.maxSpeed, (i) => aIPath.maxSpeed = i, (i) => aIPath.maxSpeed = i));
                    break;
                case EffectType.dot:
                    Health enemyHealth = enemy.GetComponent<Health>();
                    StartCoroutine(Dot(enemyHealth));
                    break;
                case EffectType.weakness:
                    DamageManager enemyDamage = enemy.GetComponent<DamageManager>();
                    StartCoroutine(Weakness(enemyDamage.damage, (i) => enemyDamage.damage = i, (i) => enemyDamage.damage = i));
                    break;
            }
        }
    }

    public IEnumerator Slowness(float startValue, Action<float> changeValue, Action<float> resetValue)
    {
        Effect slownessEffect = GetEffectFromType(EffectType.slowness);

        StartTimer(slownessEffect);

        float currentValue = startValue;
        currentValue -= PowFromValue(slownessEffect.effectPower, startValue, 11);
        changeValue(currentValue);

        yield return new WaitForSeconds(slownessEffect.duration);

        resetValue(startValue);
    }

    public IEnumerator Dot(Health health)
    {
        Effect dotEffect = GetEffectFromType(EffectType.dot);

        StartTimer(dotEffect);

        float dotDamage = PowFromValue(dotEffect.effectPower, PowFromValue(2.5f, health.maxHealth, 100), 11);

        while (dotEffect.effectActive)
        {
            yield return new WaitForSeconds(.4f);

            if (health != null)
            {
                health.RemoveHealth(dotDamage, null);
                health.damagePopUp.PopUp(dotDamage, .25f);
            }
        }
    }

    public IEnumerator Weakness(float startValue, Action<float> changeValue, Action<float> resetValue)
    {
        Effect weaknessEffect = GetEffectFromType(EffectType.weakness);

        StartTimer(weaknessEffect);

        float currentValue = startValue;
        currentValue -= PowFromValue(weaknessEffect.effectPower, startValue, 11);
        changeValue(currentValue);

        yield return new WaitForSeconds(weaknessEffect.duration);

        resetValue(startValue);
    }

    void StartTimer(Effect effect)
    {
        effect.effectActive = true;
        effect.currentTime = effect.duration;
        StatsManager.instance.canModifyStats = false;

        StopCoroutine(Timer(effect));
        StartCoroutine(Timer(effect));
    }

    IEnumerator Timer(Effect effect)
    {
        while (effect.currentTime > 0)
        {
            effect.currentTime -= Time.deltaTime;

            yield return null;
        }

        effect.effectActive = false;
        StatsManager.instance.canModifyStats = true;
    }

    float PowFromValue(float powRange, float value, float maxPercent)
    {
        return powRange * value / maxPercent;
    }

    Effect GetEffectFromType(EffectType type)
    {
        return effects.Find(x => x.type == type);
    }
}

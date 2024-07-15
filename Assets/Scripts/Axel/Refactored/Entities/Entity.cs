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
        health, damage, speed, defense, attackSpeed
    };

    [HideInInspector] public float health;
    [HideInInspector] public float healthCurrent;
    [HideInInspector] public float healthMultiplier;
    [HideInInspector] public float damage;
    [HideInInspector] public float damageCurrent;
    [HideInInspector] public float damageMultiplier;
    [HideInInspector] public float speed;
    [HideInInspector] public float speedCurrent;
    [HideInInspector] public float speedMultiplier;
    [HideInInspector] public float defense;
    [HideInInspector] public float defenseCurrent;
    [HideInInspector] public float defenseMultiplier;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float attackSpeedCurrent;
    [HideInInspector] public float attackSpeedMultiplier;
    [HideInInspector] public bool isMobile;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canDealDamage;
    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public bool appliesEffects;
    [HideInInspector] public List<Effect> effects = new List<Effect>();
    [HideInInspector] public GameObject effectsHolder;
    [HideInInspector] public GameObject popUpMessage;

    public FieldInfo[] properties = typeof(Entity).GetFields();

    public virtual void Awake()
    {
        healthCurrent = health;
        speedCurrent = speed;
        attackSpeedCurrent = attackSpeed;
        defenseCurrent = defense;
        damageCurrent = damage;

        effectsHolder = Instantiate(new GameObject("EffectsHolder"), transform);
    }

    public virtual void Start()
    {
        SetStat(Stat.health, 5);
    }

    public void AddHealth(float healthToAdd)
    {
        if (healthCurrent < health)
        {
            healthCurrent += healthToAdd;

            PopUpMessage("" + healthToAdd, .3f, Color.green, 0);
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

            PopUpMessage("" + healthToRemove, .3f, Color.red, 0);

            if (healthCurrent <= 0)
            {
                healthCurrent = 0;

                gameObject.SetActive(false);
            }
        }
    }

    public void SetStat(Stat stat, float statChange)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat));
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Current");

        float changedValue = (float)property.GetValue(this) + statChange;
        float currentChangedValue = (float)currentProperty.GetValue(this) + statChange;

        property.SetValue(this, changedValue);
        currentProperty.SetValue(this, currentChangedValue);
    }
    public float GetStat(Stat stat)
    {
        FieldInfo property = properties.ToList().Find(p => p.Name == GetStatName(stat));

        return (float)property.GetValue(this);
    }

    public float GetCurrentStat(Stat stat)
    {
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Current");

        return (float)currentProperty.GetValue(this);
    }

    public float GetStatMultiplier(Stat stat)
    {
        FieldInfo currentProperty = properties.ToList().Find(p => p.Name == GetStatName(stat) + "Multiplier");

        return (float)currentProperty.GetValue(this);
    }

    string GetStatName(Stat stat)
    {
        return stat.ToString();
    }

    public void ApplyEffect(Entity entityAttacker)
    {
        List<string> effects = new List<string>();

        int index = 0;

        foreach (Effect effect in entityAttacker.effects)
        {
            bool hasEffect = false;

            foreach (Transform currentEffect in effectsHolder.transform)
            {
                if (effect.type == currentEffect.GetComponent<Effect>().type) hasEffect = true;
            }

            if (hasEffect) continue;

            GameObject instanceEffect = Resources.Load($"Effects/{effect.type}", typeof(GameObject)) as GameObject;
            instanceEffect.name = "" + effect.type;

            effects.Add("" + effect.type);

            Effect newEffect = Instantiate(instanceEffect, effectsHolder.transform).GetComponent<Effect>();

            newEffect.effectDuration = effect.effectDuration;
            newEffect.effectPower = effect.effectPower;
            newEffect.type = effect.type;
            newEffect.entity = this;

            index++;
        }

        StartCoroutine(DisplayEffects(effects, .5f));
    }

    IEnumerator DisplayEffects(List<string> messages, float dealy)
    {
        foreach (string message in messages)
        {
            PopUpMessage(message, .2f, Color.yellow, 1);

            yield return new WaitForSeconds(dealy);
        }

        messages.Clear();
    }

    public void PopUpMessage(string message, float fontSize, Color messageColor, float offset)
    {
        GameObject instance = Instantiate(popUpMessage, transform);
        TextMeshProUGUI messageText = instance.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

        instance.transform.localPosition += Vector3.up * offset;

        Color newColor = new Color(messageColor.r, messageColor.g, messageColor.b, .1f);

        messageText.text = message;
        messageText.color = newColor;
        messageText.fontSize = fontSize;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [HideInInspector] public float health;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float damage;
    [HideInInspector] public float currentDamage;
    [HideInInspector] public float speed;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float defense;
    [HideInInspector] public float currentDefese;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float currentAttackSpeed;
    [HideInInspector] public bool isMobile;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canDealDamage;
    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public bool appliesEffects;
    [HideInInspector] public List<Effect> effects = new List<Effect>();
    [HideInInspector] public GameObject popUpMessage;

    public virtual void Awake()
    {
        currentHealth = health;
        currentSpeed = speed;
        currentAttackSpeed = attackSpeed;
        currentDefese = defense;
        currentDamage = damage;

    }

    public void AddHealth(float healthToAdd)
    {
        if (currentHealth < health)
        {
            currentHealth += healthToAdd;

            PopUpMessage("" + healthToAdd, .3f, Color.green, 0);
        }
        else
        {
            currentHealth = health;
        }
    }

    public void DealDamage(float healthToRemove)
    {
        if (currentHealth > 0)
        {
            currentHealth -= healthToRemove;

            PopUpMessage("" + healthToRemove, .3f, Color.red, 0);

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                gameObject.SetActive(false);
            }
        }
    }

    public void ApplyEffect(Entity entityAttacker)
    {
        List<string> effects = new List<string>();

        foreach (Effect effect in entityAttacker.effects)
        {
            GameObject instanceEffect = Resources.Load($"Effects/{effect.type}", typeof(GameObject)) as GameObject;
            instanceEffect.name = "" + effect.type;

            effects.Add("" + effect.type);

            Effect newEffect = Instantiate(instanceEffect, transform).GetComponent<Effect>();

            newEffect.effectDuration = effect.effectDuration;
            newEffect.effectPower = effect.effectPower;

            newEffect.entity = this;
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

using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;
    [Space]
    public GameObject onDeathVFX;
    [Space]

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
    [HideInInspector] public bool canGetKnockback = true;
    [HideInInspector] public EffectManager effectsManager;

    public enum Stat
    {
        health, defense, speed, damage, attackSpeed
    };

    public virtual void Awake()
    {
        healthCurrent = health;
        speedCurrent = speed;
        attackSpeedCurrent = attackSpeed;
        defenseCurrent = defense;
        damageCurrent = damage;

        if (onDeathVFX == null) onDeathVFX = Resources.Load("VFX/Puff") as GameObject;
    }

    public virtual void Start() { }

    public virtual void Update()
    {
        if(healthCurrent <= 0)
        {
            OnDeath();
        }

        if (!canMove) return;
    }

    public virtual void OnDeath()
    {
        Audio.instance.PlayOneShot(Audio.Sound.squash, .15f);

        Instantiate(onDeathVFX, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
        
    public virtual void AddHealth(float healthToAdd)
    {
        if (healthCurrent < health)
        {
            float actualHealthToAdd = healthToAdd;

            if(healthCurrent + healthToAdd > health)
            {
                actualHealthToAdd = health - healthCurrent;
            }

            healthCurrent += actualHealthToAdd;

            Audio.instance.PlayOneShot(Audio.Sound.heal, .3f);

            PopUp.instance.Message(transform, "" + Math.Round(actualHealthToAdd, 1), Color.green, .5f, true);
        }
    }

    public virtual void RemoveHealth(float healthToRemove)
    {
        if (healthCurrent > 0)
        {
            float healthDefensed = defenseCurrent * (healthToRemove / 2) / health;
            float actualHealthToRemove = healthToRemove - healthDefensed;

            healthCurrent -= actualHealthToRemove;

            PopUp.instance.Message(transform, "" + Math.Round(actualHealthToRemove, 1), Color.red, (defenseCurrent < defense) ? .65f : .4f, true);

            if (healthCurrent <= 0)
            {
                healthCurrent = 0;

                canMove = false;
            }
        }
    }

    public virtual IEnumerator JiggleAnimation(int power)
    {
        Vector3 initialScale = transform.localScale;
        float duration = .5f;
        float currentTime = 0;
        float offset = .1f;

        while (currentTime < duration / 3)
        {
            currentTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(initialScale, new Vector3(initialScale.x - offset, initialScale.y - offset, 0), currentTime / (duration / 3));

            yield return null;
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, Vector4.one, currentTime / duration);

            yield return null;
        }

        transform.localScale = initialScale;
    }
}

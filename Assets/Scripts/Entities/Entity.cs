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

    [HideInInspector] public SpriteRenderer _spriteRenderer;

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

            if (actualHealthToAdd > 0) PopUp.instance.Message(transform, "" + Math.Round(actualHealthToAdd, 1), Color.green, .5f, true);
        }
    }

    public virtual void RemoveHealth(float healthToRemove)
    {
        if (healthCurrent > 0)
        {
            float healthDefensed = defenseCurrent * (healthToRemove / 2) / health;
            float actualHealthToRemove = healthToRemove - healthDefensed;

            healthCurrent -= actualHealthToRemove;

            if (actualHealthToRemove > 0) PopUp.instance.Message(transform, "" + Math.Round(actualHealthToRemove, 1), Color.red, (defenseCurrent < defense) ? .65f : .4f, true);

            if (healthCurrent <= 0)
            {
                healthCurrent = 0;

                canMove = false;
            }
        }
    }

    public IEnumerator DamagedAnimation(float duration)
    {
        float currentTime = 0;

        Material entityMat = _spriteRenderer.material;

        //float firstDuration = duration * .25f;

        //while (currentTime < firstDuration)
        //{
        //    currentTime += Time.deltaTime;

        //    entityMat.SetFloat("_BlendFactor", Mathf.Clamp(Mathf.Lerp(1, 0, currentTime / firstDuration), 0, 1));

        //    yield return null;
        //}

        entityMat.SetFloat("_BlendFactor", 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            entityMat.SetFloat("_BlendFactor", Mathf.Clamp(Mathf.Lerp(1, 0, currentTime / duration), 0, 1));

            yield return null;
        }

        entityMat.SetFloat("_BlendFactor", 0);
    }

    public virtual IEnumerator JiggleAnimation(float duration)
    {
        Vector3 initialScale = Vector3.one;
        float currentTime = 0;
        float offset = .1f;

        float firstDuration = duration * .33f;

        transform.localScale = initialScale;

        while (currentTime < firstDuration)
        {
            currentTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(initialScale, new Vector3(initialScale.x - offset, initialScale.y - offset, 0), currentTime / firstDuration);

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

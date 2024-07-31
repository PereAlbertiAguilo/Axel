using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public bool appliesEffects;
    public Entity entity;
    public GameObject effectsHolder;
    public EffectParameters parameters;

    bool canApplyEffect = true;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();

        effectsHolder = new GameObject("EffectsHolder");
        effectsHolder.transform.SetParent(transform);
    }

    public void ApplyEffect(EffectManager attackerEffects)
    {
        bool hasEffect = false;

        foreach (Transform currentEffect in effectsHolder.transform)
        {
            if (attackerEffects.parameters.type == currentEffect.GetComponent<Effect>().parameters.type) hasEffect = true;
        }

        if (hasEffect || !attackerEffects.canApplyEffect) return;

        attackerEffects.canApplyEffect = false;

        GameObject instanceEffect = Resources.Load($"Effects/{attackerEffects.parameters.type}", typeof(GameObject)) as GameObject;
        instanceEffect.name = "" + attackerEffects.parameters.type;

        Effect newEffect = Instantiate(instanceEffect, effectsHolder.transform).GetComponent<Effect>();
        newEffect.parameters = attackerEffects.parameters;
        newEffect.entity = entity;

        PopUp.instance.Message(transform.parent, $"{newEffect.parameters.type} {Math.Round(newEffect.parameters.duration, 1)} s", Color.yellow, .4f, 1, true);

        attackerEffects.StartCoroutine(attackerEffects.EffectCooldown(newEffect.parameters.duration + newEffect.parameters.cooldown, attackerEffects));
    }

    public IEnumerator EffectCooldown(float cooldown, EffectManager attackerEffects)
    {
        yield return new WaitForSeconds(cooldown);

        if (attackerEffects != null)
        {
            PopUp.instance.Message(attackerEffects.transform.parent, $"{attackerEffects.parameters.type}", Color.green, .4f, 1, true);

            attackerEffects.canApplyEffect = true;
        }
    }
}
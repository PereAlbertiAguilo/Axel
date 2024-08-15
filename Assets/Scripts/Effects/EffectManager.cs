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
        entity.effectsManager = this;

        effectsHolder = new GameObject("EffectsHolder");
        effectsHolder.transform.SetParent(transform);
    }

    public void ApplyEffect(EffectManager attackerEffectManager)
    {
        bool hasEffect = false;

        foreach (Transform currentEffect in effectsHolder.transform)
        {
            if (attackerEffectManager.parameters.type == currentEffect.GetComponent<Effect>().parameters.type) hasEffect = true;
        }

        if (hasEffect || !attackerEffectManager.canApplyEffect || attackerEffectManager == null) return;

        attackerEffectManager.canApplyEffect = false;

        GameObject instanceEffect = Resources.Load($"Effects/{attackerEffectManager.parameters.type}", typeof(GameObject)) as GameObject;
        instanceEffect.name = "" + attackerEffectManager.parameters.type;

        Effect newEffect = Instantiate(instanceEffect, effectsHolder.transform).GetComponent<Effect>();
        newEffect.parameters = attackerEffectManager.parameters;
        newEffect.entity = entity;
        newEffect.attakerEntity = attackerEffectManager.entity;

        PopUp.instance.Effect(transform.parent, newEffect.parameters.type, Color.cyan, 2, 1, true, newEffect.parameters.duration, false);
        PopUp.instance.Message(transform.parent, $"{newEffect.parameters.type}", Color.cyan, .4f, 2f, true);

        attackerEffectManager.StartCoroutine(attackerEffectManager.EffectCooldown(newEffect.parameters.duration + newEffect.parameters.cooldown, attackerEffectManager));
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
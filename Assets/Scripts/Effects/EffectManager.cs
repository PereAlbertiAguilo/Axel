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
            if (currentEffect.GetComponent<Effect>() != null) hasEffect = true;
        }

        if (hasEffect || !attackerEffectManager.canApplyEffect || attackerEffectManager == null || !attackerEffectManager.gameObject.activeInHierarchy) return;

        attackerEffectManager.canApplyEffect = false;

        GameObject instanceEffect = Resources.Load($"Effects/{attackerEffectManager.parameters.type}", typeof(GameObject)) as GameObject;
        instanceEffect.name = "" + attackerEffectManager.parameters.type;

        Effect newEffect = Instantiate(instanceEffect, effectsHolder.transform).GetComponent<Effect>();
        newEffect.parameters = attackerEffectManager.parameters;
        newEffect.targetEntity = entity;
        newEffect.attackerEntity = attackerEffectManager.entity;

        DisplayEffectPopUp(newEffect);

        attackerEffectManager.StartCoroutine(attackerEffectManager.EffectCooldown(newEffect.parameters.duration + newEffect.parameters.cooldown, attackerEffectManager));
    }

    public IEnumerator EffectCooldown(float cooldown, EffectManager attackerEffects)
    {
        yield return new WaitForSeconds(cooldown);

        if (attackerEffects != null )
        {
            PopUp.instance.Message(attackerEffects.transform.parent, $"{attackerEffects.parameters.type}", Color.green, .4f, 1, true);

            attackerEffects.canApplyEffect = true;
        }
    }

    void DisplayEffectPopUp(Effect newEffect)
    {
        float offset = transform.parent.GetComponent<Collider2D>().bounds.size.y + .5f;

        PopUp.instance.Effect(transform.parent, newEffect.parameters.type, Color.cyan, 2, offset, true, newEffect.parameters.duration, false);
        PopUp.instance.Message(transform.parent, $"{newEffect.parameters.type}", Color.cyan, .4f, offset + .75f, true);
    }
}
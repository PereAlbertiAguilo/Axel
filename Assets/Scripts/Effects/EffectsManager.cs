using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [HideInInspector] public bool appliesEffects;
    [HideInInspector] public Entity entity;
    [HideInInspector] public GameObject effectsHolder;
    [HideInInspector] public List<EffectParameters> appliableEffects = new List<EffectParameters>();
    [HideInInspector] public EffectParameters.Type effectType;
    [Range(1, 10)]
    [HideInInspector] public int effectPower = 1;
    [HideInInspector] public float effectDuration = 1;
    private void Awake()
    {
        entity = GetComponentInParent<Entity>();

        effectsHolder = new GameObject("EffectsHolder");
        effectsHolder.transform.SetParent(transform);
    }

    public void ApplyEffect(EffectsManager attackerEffects)
    {
        List<string> effects = new List<string>();
        List<float> durations = new List<float>();

        foreach (EffectParameters effectParameters in attackerEffects.appliableEffects)
        {
            bool hasEffect = false;

            foreach (Transform currentEffect in effectsHolder.transform)
            {
                if (effectParameters.type == currentEffect.GetComponent<Effect>().effectParameters.type) hasEffect = true;
            }

            if (hasEffect) continue;

            GameObject instanceEffect = Resources.Load($"Effects/{effectParameters.type}", typeof(GameObject)) as GameObject;
            instanceEffect.name = "" + effectParameters.type;

            Effect newEffect = Instantiate(instanceEffect, effectsHolder.transform).GetComponent<Effect>();

            newEffect.effectParameters.duration = effectParameters.duration;
            newEffect.effectParameters.power = effectParameters.power;
            newEffect.effectParameters.type = effectParameters.type;
            newEffect.entity = entity;

            effects.Add("" + newEffect.effectParameters.type);
            durations.Add(newEffect.effectParameters.duration);
        }

        DisplayEffects(effects, durations);
    }

    void DisplayEffects(List<string> messages, List<float> durations)
    {
        for (int i = 0; i < messages.Count; i++)
        {
            PopUp.instance.Message(transform.parent, messages[i], Color.yellow, .2f, durations[i], true, 1 + (.2f * i));
        }

        messages.Clear();
        durations.Clear();
    }
}

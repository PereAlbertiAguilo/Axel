using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CustomEditor(typeof(Entity), true)]
[CanEditMultipleObjects]
public class EntityEditor : Editor
{
    SerializedProperty health, damage, speed, defense, attackSpeed, isMobile, canDealDamage, appliesEffects, effects, popUpMessage, 
        healthMultiplier, damageMultiplier, speedMultiplier, defenseMultiplier, attackSpeedMultiplier;

    static bool foldout = false;
    static bool multiplierFoldout = false;
    static bool effectsFoldout = false;

    static Effect.EffectType effectType;

    int effectPower = 1;

    float effectDuration = 1;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        speed = serializedObject.FindProperty("speed");
        defense = serializedObject.FindProperty("defense");
        attackSpeed = serializedObject.FindProperty("attackSpeed");
        isMobile = serializedObject.FindProperty("isMobile");
        canDealDamage = serializedObject.FindProperty("canDealDamage");
        appliesEffects = serializedObject.FindProperty("appliesEffects");
        effects = serializedObject.FindProperty("effects");
        popUpMessage = serializedObject.FindProperty("popUpMessage");
        healthMultiplier = serializedObject.FindProperty("healthMultiplier");
        damageMultiplier = serializedObject.FindProperty("damageMultiplier");
        speedMultiplier = serializedObject.FindProperty("speedMultiplier");
        defenseMultiplier = serializedObject.FindProperty("defenseMultiplier");
        attackSpeedMultiplier = serializedObject.FindProperty("attackSpeedMultiplier");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Entity entity = (Entity)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        foldout = EditorGUILayout.Foldout(foldout, "Entity Stats", true, EditorStyles.foldoutHeader);

        if(foldout)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isMobile);
            EditorGUILayout.PropertyField(canDealDamage);
            EditorGUILayout.PropertyField(appliesEffects);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(health);
            EditorGUILayout.PropertyField(defense);

            if (entity.isMobile) EditorGUILayout.PropertyField(speed);

            if (entity.canDealDamage)
            {
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(attackSpeed);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            multiplierFoldout = EditorGUILayout.Foldout(multiplierFoldout, "Stats Multiplier", true, EditorStyles.foldoutHeader);

            if (multiplierFoldout)
            {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(healthMultiplier);
                EditorGUILayout.PropertyField(defenseMultiplier);

                if (entity.isMobile) EditorGUILayout.PropertyField(speedMultiplier);

                if (entity.canDealDamage)
                {
                    EditorGUILayout.PropertyField(damageMultiplier);
                    EditorGUILayout.PropertyField(attackSpeedMultiplier);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (entity.canDealDamage && entity.appliesEffects)
            {
                effectsFoldout = EditorGUILayout.Foldout(effectsFoldout, "Effects Manager", true, EditorStyles.foldoutHeader);

                if (effectsFoldout)
                {
                    effectPower = EditorGUILayout.IntSlider("Effect Power", effectPower, 1, 10);

                    effectDuration = EditorGUILayout.FloatField("Effect Duration", effectDuration);

                    effectType = (Effect.EffectType)EditorGUILayout.EnumPopup("Effect Type", effectType);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(effects);

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Add Effect"))
                    {
                        CreateEffect(effectType, effectPower, effectDuration, entity);
                    }
                    if (GUILayout.Button("Remove Effect"))
                    {
                        RemoveEffect(effectType, entity);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(popUpMessage);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }

    void CreateEffect(Effect.EffectType effectType, int effectPower, float effectDuration, Entity entity)
    {
        foreach (Effect effect in entity.effects)
        {
            if (effect.type == effectType)
            {
                Debug.Log($"Effect {effectType} is already in use");
                return;
            }
        }

        GameObject instanceEffect = CreateEffectInstance(effectType);

        if (instanceEffect != null)
        {
            GameObject holder = entity.transform.Find("EffectsHolder").gameObject;

            Effect newEffect = Instantiate(instanceEffect, holder.transform).GetComponent<Effect>();

            newEffect.effectDuration = effectDuration;
            newEffect.effectPower = effectPower;
            newEffect.type = effectType;

            entity.effects.Add(newEffect);

            newEffect.gameObject.name = "" + effectType;
            newEffect.gameObject.SetActive(false);

            Debug.Log("Effect created successfully");
        }
        else
        {
            Debug.Log("Effect not existent");
        }
    }

    void RemoveEffect(Effect.EffectType effectType, Entity entity)
    {
        GameObject holder = entity.transform.Find("EffectsHolder").gameObject;

        foreach (Transform instance in holder.transform)
        {
            if(instance.name == "" + effectType)
            {
                foreach (Effect effect in entity.effects)
                {
                    if (effect.type == effectType) entity.effects.Remove(effect); break;
                }

                DestroyImmediate(instance.gameObject);

                Debug.Log("Effect removed successfully");
            }
        }
    }

    GameObject CreateEffectInstance(Effect.EffectType effectType)
    {
        return Resources.Load($"Effects/{effectType}", typeof(GameObject)) as GameObject;
    }
}
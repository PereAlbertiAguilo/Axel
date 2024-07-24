using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectsManager))]
[CanEditMultipleObjects]
public class EffectsManagerEditor : Editor
{
    SerializedProperty appliesEffects, entity, appliableEffects, effectType, effectPower, effectDuration;

    static bool effectsFoldout = false;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        appliesEffects = serializedObject.FindProperty("appliesEffects");
        entity = serializedObject.FindProperty("entity");
        appliableEffects = serializedObject.FindProperty("appliableEffects");
        effectType = serializedObject.FindProperty("effectType");
        effectPower = serializedObject.FindProperty("effectPower");
        effectDuration = serializedObject.FindProperty("effectDuration");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EffectsManager effectsManager = (EffectsManager)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(appliesEffects);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(entity);
        EditorGUILayout.PropertyField(appliableEffects);

        EditorGUILayout.Space();

        if (effectsManager.appliesEffects)
        {
            effectsFoldout = EditorGUILayout.Foldout(effectsFoldout, "Effects Manager", true, EditorStyles.toolbarButton);

            EditorGUILayout.Space();

            if (effectsFoldout)
            {
                EditorGUILayout.PropertyField(effectPower);
                EditorGUILayout.PropertyField(effectDuration);
                EditorGUILayout.PropertyField(effectType);

                EditorGUILayout.Space();

                for (int i = 0; i < effectsManager.appliableEffects.Count; i++)
                {
                    EditorGUILayout.LabelField($"{i} - {effectsManager.appliableEffects[i].name}", EditorStyles.toolbarButton);
                    EditorGUILayout.Space();
                }

                if (!effectsManager.appliableEffects.Exists(x => x.type == effectsManager.effectType))
                {
                    if (GUILayout.Button("Add Effect"))
                    {
                        CreateEffect(effectsManager);
                    }
                }
                else if (effectsManager.appliableEffects.Count == System.Enum.GetValues(typeof(EffectParameters.Type)).Length)
                {
                    EditorGUILayout.LabelField($"Can't add more effects to this entity", EditorStyles.toolbarButton);
                }
                else
                {
                    EditorGUILayout.LabelField($"Effect {effectsManager.effectType} is already in use", EditorStyles.toolbarButton);
                }

                if (effectsManager.appliableEffects.Count > 0)
                {
                    if (GUILayout.Button("Remove Effects"))
                    {
                        appliableEffects.ClearArray();
                    }
                }
            }
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }

    public void CreateEffect(EffectsManager effectsManager)
    {
        foreach (EffectParameters effectParameters in effectsManager.appliableEffects)
        {
            if (effectsManager.appliableEffects.Count == System.Enum.GetValues(typeof(EffectParameters.Type)).Length)
            {
                return;
            }
            if (effectParameters.type == effectsManager.effectType)
            {
                return;
            }
        }

        EffectParameters newEffectParameters = new EffectParameters()
        {
            name = effectsManager.effectType.ToString(),
            type = effectsManager.effectType,
            power = effectsManager.effectPower,
            duration = effectsManager.effectDuration
        };

        effectsManager.appliableEffects.Add(newEffectParameters);

        effectPower.intValue = newEffectParameters.power;
        //serializedObject.CopyFromSerializedProperty(appliableEffects);
    }
}

using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectManager))]
[CanEditMultipleObjects]
public class EffectsManagerEditor : Editor
{
    SerializedProperty appliesEffects, entity, parameters;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        appliesEffects = serializedObject.FindProperty("appliesEffects");
        entity = serializedObject.FindProperty("entity");
        parameters = serializedObject.FindProperty("parameters");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EffectManager effectsManager = (EffectManager)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(appliesEffects);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(entity);

        if (effectsManager.appliesEffects)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(parameters);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

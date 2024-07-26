using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Entity), true)]
[CanEditMultipleObjects]
public class EntityEditor : Editor
{
    SerializedProperty health, damage, speed, defense, attackSpeed, isMobile, canDealDamage, effectsManager;

    static bool foldout = false;

    

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
        effectsManager = serializedObject.FindProperty("effectsManager");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Entity entity = (Entity)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        foldout = EditorGUILayout.Foldout(foldout, "Entity Stats", true, EditorStyles.toolbarButton);

        if(foldout)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isMobile);
            EditorGUILayout.PropertyField(canDealDamage);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(health);
            EditorGUILayout.PropertyField(defense);
            defense.floatValue = (defense.floatValue >= health.floatValue * 2) ? (health.floatValue * 2) : (defense.floatValue <= 0 ? 0 : defense.floatValue);

            if (entity.isMobile) EditorGUILayout.PropertyField(speed);

            if (entity.canDealDamage)
            {
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(attackSpeed);

                EditorGUILayout.PropertyField(effectsManager);
            }

            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }

    GUILayoutOption[] DefenseOptions(Entity entity)
    {
        GUILayoutOption[] defenseOptions = new GUILayoutOption[]
        {

        };

        return defenseOptions;
    }
}
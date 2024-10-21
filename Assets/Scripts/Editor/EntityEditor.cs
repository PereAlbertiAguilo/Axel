using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Entity), true)]
[CanEditMultipleObjects]
public class EntityEditor : Editor
{
    SerializedProperty health, damage, speed, defense, attackSpeed, isMobile, canDealDamage, canGetKnockback, effectsManager,
        healthCurrent, damageCurrent, speedCurrent, defenseCurrent, attackSpeedCurrent;

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
        canGetKnockback = serializedObject.FindProperty("canGetKnockback");
        effectsManager = serializedObject.FindProperty("effectsManager");

        healthCurrent = serializedObject.FindProperty("healthCurrent");
        damageCurrent = serializedObject.FindProperty("damageCurrent");
        speedCurrent = serializedObject.FindProperty("speedCurrent");
        defenseCurrent = serializedObject.FindProperty("defenseCurrent");
        attackSpeedCurrent = serializedObject.FindProperty("attackSpeedCurrent");
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
            if(entity.isMobile) EditorGUILayout.PropertyField(canGetKnockback);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(health);
            GUILayout.Label("Current Health: " + entity.healthCurrent);

            EditorGUILayout.PropertyField(defense);
            GUILayout.Label("Current Defense: " + entity.defenseCurrent);

            defense.floatValue = (defense.floatValue >= health.floatValue * 2) ? (health.floatValue * 2) : (defense.floatValue <= 0 ? 0 : defense.floatValue);

            if (entity.isMobile)
            {
                EditorGUILayout.PropertyField(speed);
                GUILayout.Label("Current Speed: " + entity.speedCurrent);
            }

            if (entity.canDealDamage)
            {
                EditorGUILayout.PropertyField(damage);
                GUILayout.Label("Current Damage: " + entity.damageCurrent);
                EditorGUILayout.PropertyField(attackSpeed);
                GUILayout.Label("Current Attack Speed: " + entity.attackSpeedCurrent);

                EditorGUILayout.Space();

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
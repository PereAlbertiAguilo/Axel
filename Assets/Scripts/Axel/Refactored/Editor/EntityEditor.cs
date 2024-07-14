using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Entity), true)]
[CanEditMultipleObjects]
public class EntityEditor : Editor
{
    SerializedProperty health, damage, speed, defense, attackSpeed, isMobile, canMove, canDealDamage, appliesEffects, effects, popUpMessage;

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
        canMove = serializedObject.FindProperty("canMove");
        canDealDamage = serializedObject.FindProperty("canDealDamage");
        appliesEffects = serializedObject.FindProperty("appliesEffects");
        effects = serializedObject.FindProperty("effects");
        popUpMessage = serializedObject.FindProperty("popUpMessage");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Entity entity = (Entity)target;

        EditorGUILayout.Space();

        foldout = EditorGUILayout.Foldout(foldout, "Entity Stats", true, EditorStyles.foldoutHeader);

        if(foldout)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(health);
            EditorGUILayout.PropertyField(defense);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isMobile);

            EditorGUILayout.Space();

            if (entity.isMobile)
            {
                EditorGUILayout.PropertyField(canMove);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(speed);
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(canDealDamage);

            EditorGUILayout.Space();

            if (entity.canDealDamage)
            {
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(attackSpeed);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(appliesEffects);

                if (entity.appliesEffects)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(effects);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(popUpMessage);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
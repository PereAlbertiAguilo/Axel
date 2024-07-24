using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Weapon;

[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    SerializedProperty weaponElement, weaponType, weaponRenderer, attackSpriteSheet, weaponName, weaponSprite;

    static bool UIContentsFoldout = false;
    static bool animationFoldout = false;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        weaponElement = serializedObject.FindProperty("weaponElement");
        weaponType = serializedObject.FindProperty("weaponType");
        weaponRenderer = serializedObject.FindProperty("weaponRenderer");
        attackSpriteSheet = serializedObject.FindProperty("attackSpriteSheet");
        weaponName = serializedObject.FindProperty("weaponName");
        weaponSprite = serializedObject.FindProperty("weaponSprite");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Weapon weapon = (Weapon)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(weaponElement);
        EditorGUILayout.PropertyField(weaponType);

        EditorGUILayout.Space();

        animationFoldout = EditorGUILayout.Foldout(animationFoldout, "Animation", true, EditorStyles.foldoutHeader);

        if (animationFoldout)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(attackSpriteSheet);
            EditorGUILayout.PropertyField(weaponRenderer);
        }

        EditorGUILayout.Space();

        UIContentsFoldout = EditorGUILayout.Foldout(UIContentsFoldout, "UI Contents", true, EditorStyles.foldoutHeader);

        if (UIContentsFoldout)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(weaponName);
            EditorGUILayout.PropertyField(weaponSprite);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

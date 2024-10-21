using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Collectable))]
[CanEditMultipleObjects]
public class CollectableEditor : Editor
{
    SerializedProperty type, amount, pickUpSound, volumeScale;

    static bool foldout = false;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        type = serializedObject.FindProperty("type");
        amount = serializedObject.FindProperty("amount");
        pickUpSound = serializedObject.FindProperty("pickUpSound");
        volumeScale = serializedObject.FindProperty("volumeScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        Collectable collectable = (Collectable)target;
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(type);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(amount);

        EditorGUILayout.Space();

        foldout = EditorGUILayout.Foldout(foldout, "SFX Parameters", true, EditorStyles.toolbarButton);

        if (foldout)
        {
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(pickUpSound);

            if (collectable.pickUpSound != null)
            {
                EditorGUILayout.PropertyField(volumeScale);
            }
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

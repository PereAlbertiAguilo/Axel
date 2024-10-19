using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomCollectable))]
[CanEditMultipleObjects]
public class RoomCollectibleEditor : Editor
{
    SerializedProperty tier, collectableTierOne, collectableTierTwo, collectableTierThree;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        tier = serializedObject.FindProperty("tier");
        collectableTierOne = serializedObject.FindProperty("collectableTierOne");
        collectableTierTwo = serializedObject.FindProperty("collectableTierTwo");
        collectableTierThree = serializedObject.FindProperty("collectableTierThree");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        RoomCollectable roomCollectible = (RoomCollectable)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(tier);

        EditorGUILayout.Space();

        switch (roomCollectible.tier)
        {
            case 1:
                EditorGUILayout.PropertyField(collectableTierOne);
                break;
            case 2:
                EditorGUILayout.PropertyField(collectableTierTwo);
                break;
            case 3:
                EditorGUILayout.PropertyField(collectableTierThree);
                break;
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

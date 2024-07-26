using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnRepeating), true)]
public class SpawnRepeatingEditor : Editor
{
    SerializedProperty aimToPlayer, directionX, directionY, shootingObject, poolSize;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        aimToPlayer = serializedObject.FindProperty("aimToPlayer");
        directionX = serializedObject.FindProperty("directionX");
        directionY = serializedObject.FindProperty("directionY");
        shootingObject = serializedObject.FindProperty("shootingObject");
        poolSize = serializedObject.FindProperty("poolSize");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        SpawnRepeating spawnRepeating = (SpawnRepeating)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(shootingObject);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(poolSize);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(aimToPlayer);

        EditorGUILayout.Space();

        if (!spawnRepeating.aimToPlayer)
        {
            EditorGUILayout.Slider(directionX, -1, 1);
            EditorGUILayout.Slider(directionY, -1, 1);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

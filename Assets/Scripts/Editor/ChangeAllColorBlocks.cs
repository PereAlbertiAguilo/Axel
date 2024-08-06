using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAllColorBlocks : EditorWindow
{
    SerializedProperty colors;
    SerializedObject serializedObject;

    public ColorBlock colorBlock;
    static bool flodout = false;

    [MenuItem("Window/ChangeAllColorBlocks")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChangeAllColorBlocks));
    }

    private void OnEnable()
    {
        serializedObject = new(this);

        colorBlock = FindObjectOfType<Selectable>(true).colors;

        colors = serializedObject.FindProperty("colorBlock");
    }

    void OnGUI()
    {
        serializedObject.Update();

        flodout = EditorGUILayout.BeginFoldoutHeaderGroup(flodout, "Changing Color Block");

        if (flodout) EditorGUILayout.PropertyField(colors);

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        if (GUILayout.Button("Change All Color Blocks"))
        {
            foreach (Selectable selectable in FindObjectsOfType<Selectable>(true))
            {
                Undo.RecordObject(selectable, "colors change");
                EditorUtility.SetDirty(selectable);
                selectable.colors = colorBlock;
                EditorSceneManager.MarkSceneDirty(selectable.gameObject.scene);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
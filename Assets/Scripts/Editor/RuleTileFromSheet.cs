using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuleTileFromSheet : EditorWindow
{
    SerializedProperty _ruleTile, _spriteSheet;
    SerializedObject serializedObject;

    public RuleTile ruleTile;
    public Sprite[] spriteSheet;

    [MenuItem("Window/RuleTileFromSheet")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RuleTileFromSheet));
    }

    private void OnEnable()
    {
        serializedObject = new(this);

        _ruleTile = serializedObject.FindProperty("ruleTile");
        _spriteSheet = serializedObject.FindProperty("spriteSheet");
    }

    void OnGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(_ruleTile);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(_spriteSheet);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create New Rule Tile From Sprite Sheet"))
        {
            for (int i = 0; i < ruleTile.m_TilingRules.Count; i++)
            {
                Undo.RecordObject(ruleTile, "new rule tile");
                EditorUtility.SetDirty(ruleTile);

                Sprite[] sprite = new Sprite[1];
                sprite[0] = spriteSheet[i];
                ruleTile.m_TilingRules[i].m_Sprites = sprite;

                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
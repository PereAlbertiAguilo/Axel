using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuleTileFromSheet : EditorWindow
{
    SerializedProperty _ruleTile, _spriteSheet, _defaultRandomTile, _randomSpirtes;
    SerializedObject serializedObject;

    public RuleTile ruleTile;
    public Sprite[] spriteSheet;

    public Sprite defaultRandomTile;
    public Sprite[] randomSpirtes;

    Vector2 scrollPos = Vector2.zero;

    static RuleTileFromSheet window;

    [MenuItem("Window/RuleTileFromSheet")]
    public static void ShowWindow()
    {
        window = (RuleTileFromSheet)GetWindow(typeof(RuleTileFromSheet));
        window.minSize = new Vector2(250, 250);
        window.maxSize = new Vector2(500, 500);
    }

    private void OnEnable()
    {
        serializedObject = new(this);

        _ruleTile = serializedObject.FindProperty("ruleTile");
        _spriteSheet = serializedObject.FindProperty("spriteSheet");
        _defaultRandomTile = serializedObject.FindProperty("defaultRandomTile");
        _randomSpirtes = serializedObject.FindProperty("randomSpirtes");


    }

    void OnGUI()
    {
        serializedObject.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(_ruleTile);

        EditorGUILayout.Space();

        if(ruleTile != null)
        {
            EditorGUILayout.PropertyField(_spriteSheet);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_defaultRandomTile);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_randomSpirtes);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if(ruleTile != null)
        {
            if (spriteSheet.Length > 0)
            {
                if (GUILayout.Button("Set Rule Tile From Sprite Sheet"))
                {
                    for (int i = 0; i < ruleTile.m_TilingRules.Count; i++)
                    {
                        Undo.RecordObject(ruleTile, "Set rule tile");
                        EditorUtility.SetDirty(ruleTile);

                        Sprite[] sprite = new Sprite[1];
                        sprite[0] = spriteSheet[i];
                        ruleTile.m_TilingRules[i].m_Sprites = sprite;

                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    }
                }
            }

            if (defaultRandomTile != null && randomSpirtes.Length > 0)
            {
                if (GUILayout.Button("Set randomness to tile form index"))
                {
                    Undo.RecordObject(ruleTile, "Set randnom tile form index");
                    EditorUtility.SetDirty(ruleTile);

                    RuleTile.TilingRule tileToRandomizeIndex = ruleTile.m_TilingRules.Find(x => x.m_Sprites[0] == defaultRandomTile);

                    tileToRandomizeIndex.m_Output = RuleTile.TilingRuleOutput.OutputSprite.Random;

                    tileToRandomizeIndex.m_PerlinScale = .5f;

                    tileToRandomizeIndex.m_Sprites = null;
                    tileToRandomizeIndex.m_Sprites = SetRandomizedTile();

                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    Sprite[] SetRandomizedTile()
    {
        List<Sprite> sprites = new List<Sprite>();

        foreach (Sprite sprite in randomSpirtes)
        {
            sprites.Add(defaultRandomTile);
            sprites.Add(sprite);
        }

        return sprites.ToArray();
    }
}
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateSelectables : EditorWindow
{
    SerializedProperty _options, _colorBlock, _entry, _types;
    SerializedObject serializedObject;

    public enum Options
    {
        ColorBlock, Event
    }

    public enum Types
    {
        Button, Toggle, Slider, Scrollbar
    }

    public EventTrigger.Entry[] entrys;

    public Options options;
    public Types types;

    public ColorBlock colorBlock;

    [MenuItem("Window/UpdateSelectables")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UpdateSelectables));
    }

    private void OnEnable()
    {
        serializedObject = new(this);

        colorBlock = FindObjectOfType<Selectable>(true).colors;

        _options = serializedObject.FindProperty("options");
        _colorBlock = serializedObject.FindProperty("colorBlock");
        _entry = serializedObject.FindProperty("entrys");
        _types = serializedObject.FindProperty("types");
    }

    void OnGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_options);

        EditorGUILayout.Space();

        switch (options)
        {
            case Options.ColorBlock:
                EditorGUILayout.PropertyField(_colorBlock);
                break; 
            case Options.Event:
                EditorGUILayout.PropertyField(_entry);
                EditorGUILayout.PropertyField(_types);
                break;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Update Selectables"))
        {
            foreach (Selectable selectable in FindObjectsOfType<Selectable>(true))
            {
                Undo.RecordObject(selectable.gameObject, "Update Selectables");
                //string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selectable);
                //GameObject seleectabelPrefabObject = PrefabUtility.LoadPrefabContents(prefabPath);
                EditorUtility.SetDirty(selectable);

                selectable.colors = colorBlock;
                
                switch (options)
                {
                    case Options.ColorBlock:
                        
                        break;
                    case Options.Event:

                        if (selectable.TryGetComponent(out EventTrigger trigger))
                        {
                            DestroyImmediate(trigger);
                        }

                        //selectable.AddComponent<EventTrigger>();

                        //if(selectable.TryGetComponent(out EventTrigger eventTrigger))
                        //{
                        //    foreach (EventTrigger.Entry entry in entrys)
                        //    {
                        //        eventTrigger.triggers.Add(entry);
                        //    }
                        //}
                        
                        break;
                }

                EditorSceneManager.MarkSceneDirty(selectable.gameObject.scene);
                //PrefabUtility.SaveAsPrefabAsset(seleectabelPrefabObject, prefabPath);
                //PrefabUtility.UnloadPrefabContents(seleectabelPrefabObject);
            }
        }

        EditorGUILayout.Space();

        if(GUILayout.Button("Select Selectables"))
        {
            List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

            foreach (Selectable selectable in FindObjectsOfType<Selectable>(true))
            {
                switch (types)
                {
                    case Types.Button: 
                        if(selectable.GetType() == typeof(Button)) objects.Add(selectable.gameObject);
                        break;
                    case Types.Toggle:
                        if (selectable.GetType() == typeof(Toggle)) objects.Add(selectable.gameObject);
                        break;
                    case Types.Slider:
                        if (selectable.GetType() == typeof(Slider)) objects.Add(selectable.gameObject);
                        break;
                    case Types.Scrollbar:
                        if (selectable.GetType() == typeof(Scrollbar)) objects.Add(selectable.gameObject);
                        break;
                }
                
            }

            Selection.objects = objects.ToArray();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
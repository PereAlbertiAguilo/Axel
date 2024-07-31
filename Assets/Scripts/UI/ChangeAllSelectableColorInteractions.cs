#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.UI;

public class ChangeAllSelectableColorInteractions : MonoBehaviour
{
    public ColorBlock colors;

    static bool flodout = false;

#if UNITY_EDITOR
    [CustomEditor(typeof(ChangeAllSelectableColorInteractions))]
    [CanEditMultipleObjects]
    public class ChangeAllSelectableColorInteractionsEditor : Editor
    {
        SerializedProperty caca;

        protected virtual void OnEnable()
        {
            setvars();
        }

        void setvars()
        {
            
        }

        public override void OnInspectorGUI()
        {
            flodout = EditorGUILayout.BeginFoldoutHeaderGroup(flodout, "Changing Color Block");

            if (flodout)
            {
                base.OnInspectorGUI();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.Update();

            ChangeAllSelectableColorInteractions changeAllSelectableColorInteractions = (ChangeAllSelectableColorInteractions)target;

            EditorGUILayout.Space();

            if(GUILayout.Button("Change All Selectables Color Interations"))
            {
                foreach (Selectable selectable in FindObjectsOfType<Selectable>(true))
                {
                    Undo.RecordObject(selectable, "colors change");
                    EditorUtility.SetDirty(selectable);
                    selectable.colors = changeAllSelectableColorInteractions.colors;
                    EditorSceneManager.MarkSceneDirty(selectable.gameObject.scene);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}

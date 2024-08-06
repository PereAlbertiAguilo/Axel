using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
[CanEditMultipleObjects]
public class InteractableEditor : Editor
{
    SerializedProperty interactDelay, hasUses, uses;

    protected virtual void OnEnable()
    {
        setvars();
    }

    void setvars()
    {
        interactDelay = serializedObject.FindProperty("interactDelay");
        hasUses = serializedObject.FindProperty("hasUses");
        uses = serializedObject.FindProperty("uses");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Interactable interactable = (Interactable)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(interactDelay);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(hasUses);

        if (interactable.hasUses)
        {
            EditorGUILayout.PropertyField(uses);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

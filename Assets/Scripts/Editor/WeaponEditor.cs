using UnityEditor;

[CustomEditor(typeof(Weapon), true)]
[CanEditMultipleObjects]
public class WeaponEditor : Editor
{
    SerializedProperty weaponElement, weaponType, weaponRenderer, attackSpriteSheet, weaponName, weaponSprite, animationFrameRate;

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
        animationFrameRate = serializedObject.FindProperty("animationFrameRate");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Weapon weapon = (Weapon)target;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(weaponElement);
        EditorGUILayout.PropertyField(weaponType);

        EditorGUILayout.Space(10);

        animationFoldout = EditorGUILayout.Foldout(animationFoldout, "Animation", true, EditorStyles.toolbarButton);

        EditorGUILayout.Space(10);

        UIContentsFoldout = EditorGUILayout.Foldout(UIContentsFoldout, "UI Contents", true, EditorStyles.toolbarButton);

        if (animationFoldout)
        {
            EditorGUILayout.Space(20);

            EditorGUILayout.PropertyField(animationFrameRate);
            EditorGUILayout.PropertyField(attackSpriteSheet);
            EditorGUILayout.PropertyField(weaponRenderer);
        }
        if (UIContentsFoldout)
        {
            EditorGUILayout.Space(20);

            EditorGUILayout.PropertyField(weaponName);
            EditorGUILayout.PropertyField(weaponSprite);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

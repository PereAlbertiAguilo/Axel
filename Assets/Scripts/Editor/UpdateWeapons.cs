using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using NUnit.Framework.Constraints;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class UpdateWeapons : EditorWindow
{
    SerializedProperty _changeByElement, _options, _appliesEffects, _newAddedDamage, _newAddedAttackSpeed, _element, _type, _newEffectParameters, _newParticle, _weapons;
    SerializedObject serializedObject;

    public enum Options
    {
        Effects, WeaponStats, Particle
    };

    public Options options;

    public bool changeByElement = true;
    public bool appliesEffects = true;

    public float newAddedDamage = 0;
    public float newAddedAttackSpeed = 0;

    public WeaponManager.Element element;
    public WeaponManager.Type type;

    public EffectParameters newEffectParameters;

    public GameObject newParticle;
    public GameObject[] weapons;

    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Window/UpdateWeapons")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UpdateWeapons));
    }

    private void OnEnable()
    {
        serializedObject = new(this);

        _changeByElement = serializedObject.FindProperty("changeByElement");
        _appliesEffects = serializedObject.FindProperty("appliesEffects");
        _options = serializedObject.FindProperty("options");
        _newAddedDamage = serializedObject.FindProperty("newAddedDamage");
        _newAddedAttackSpeed = serializedObject.FindProperty("newAddedAttackSpeed");
        _element = serializedObject.FindProperty("element");
        _type = serializedObject.FindProperty("type");
        _newEffectParameters = serializedObject.FindProperty("newEffectParameters");
        _newParticle = serializedObject.FindProperty("newParticle");
        _weapons = serializedObject.FindProperty("weapons");

        weapons = Resources.LoadAll<GameObject>($"Weapons");
    }

    void OnGUI()
    {
        serializedObject.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_changeByElement);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(changeByElement ? _element : _type);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_options);
        EditorGUILayout.Space();

        switch (options)
        {
            case Options.Effects:
                EditorGUILayout.PropertyField(_appliesEffects);
                if (appliesEffects)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(_newEffectParameters);
                }
                break;

            case Options.WeaponStats: 
                EditorGUILayout.PropertyField(_newAddedDamage);
                EditorGUILayout.PropertyField(_newAddedAttackSpeed);
            break;

            case Options.Particle: 
                EditorGUILayout.PropertyField(_newParticle);
            break;
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(_weapons);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if (GUILayout.Button($"Update Weapons by {(changeByElement ? "Element" : "Type")}"))
        {
            foreach (GameObject weaponObject in weapons)
            {
                Undo.RecordObject(weaponObject, "Weapon prefab update");
                string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(weaponObject);
                GameObject prefabWeapon = PrefabUtility.LoadPrefabContents(prefabPath);

                if (prefabWeapon.TryGetComponent(out Weapon weapon))
                {
                    if (changeByElement)
                    {
                        if (weapon.weaponElement == element)
                        {
                            ActionWithOptions(weapon);
                        }
                    }
                    else
                    {
                        if (weapon.weaponType == type)
                        {
                            ActionWithOptions(weapon);
                        }
                    }
                }

                PrefabUtility.SaveAsPrefabAsset(prefabWeapon, prefabPath);
                PrefabUtility.UnloadPrefabContents(prefabWeapon);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void ActionWithOptions(Weapon weapon)
    {
        switch (options)
        {
            case Options.Effects:
                if (appliesEffects)
                {
                    weapon.GetComponent<EffectManager>().parameters = newEffectParameters;
                }
                else
                {
                    weapon.GetComponent<EffectManager>().appliesEffects = appliesEffects;
                }
                break;

            case Options.WeaponStats:
                weapon.weaponAddedDamage = newAddedDamage;
                weapon.weaponAddedAttackSpeed = newAddedAttackSpeed;
                break;

            case Options.Particle:
                weapon.GetComponent<EffectOnDestroy>().effect = newParticle;
                break;
        }
    }
}

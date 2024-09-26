using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour, IDataPersistence
{
    public GameObject savedWeaponObject;

    public enum Element
    {
        water, air, earth, fire, light, dark
    };

    public enum Type
    {
        sword, mace, axe, hammer, dagger, orb,
    };

    public static WeaponManager instance;

    float savedAddedDamage;
    float savedAddedAttackSpeed;

    int effectPower;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Weapon savedWeapon = SetWeapon(savedWeaponObject, PlayerController.instance.transform);

        savedWeapon.weaponAddedDamage = savedAddedDamage;
        savedWeapon.weaponAddedAttackSpeed = savedAddedAttackSpeed;
        savedWeapon.gameObject.GetComponent<EffectManager>().parameters.power = effectPower;
    }

    public Weapon SetWeapon(GameObject weapon, Transform holder)
    {
        foreach (Transform child in holder)
        {
            if (child.TryGetComponent(out Weapon weaponComponent))
            {
                Destroy(child.gameObject);

                break;
            }
        }

        return Instantiate(weapon, holder).GetComponent<Weapon>();
    }

    public GameObject LoadWeapon(Element element, Type weaponType)
    {
        return Resources.Load<GameObject>($"Weapons/{weaponType}/{element}");
    }

    public void LoadData(GameData data)
    {
        savedWeaponObject = LoadWeapon(data.weaponElement, data.weaponType);

        savedAddedDamage = data.weaponAddedDamage;
        savedAddedAttackSpeed = data.weaponAddedAttackSpeed;
        effectPower = data.effectPower;
    }

    public void SaveData(GameData data)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum Element
    {
        water, air, earth, fire, light, dark
    };

    public enum Type
    {
        sword, mace, axe, hammer, dagger, orb,
    };

    public static WeaponManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetWeapon(GameObject weapon, Transform holder)
    {
        foreach (Transform child in holder)
        {
            if (child.TryGetComponent(out Weapon weaponComponent))
            {
                Destroy(child.gameObject);

                break;
            }
        }

        Instantiate(weapon, holder);
    }

    public GameObject LoadWeapon(Element element, Type weaponType)
    {
        return Resources.Load<GameObject>($"Weapons/{weaponType}/{element}");
    }
}

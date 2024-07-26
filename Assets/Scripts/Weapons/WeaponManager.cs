using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum Element
    {
        water, air, earth, fire, light, dark
    };

    public enum WeaponType
    {
        sword, maces, axes, hammers, dagger, orb,
    };

    public static WeaponManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetWeapon(string weaponFolderName, Transform holder)
    {
        GameObject weapon = Resources.Load<GameObject>(weaponFolderName);

        if(holder.childCount > 0)
        {
            Destroy(holder.GetChild(0).gameObject);
        }
        
        Instantiate(weapon, holder);
    }
}

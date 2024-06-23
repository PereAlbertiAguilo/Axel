using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        mele, ranged, shield
    };

    public enum Element
    {
        water, air, earth, fire, light, dark
    };

    public WeaponType weaponType;

    EffectManager effectManager;

    private void Awake()
    {
        effectManager = GetComponent<EffectManager>();
    }

    private void Start()
    {
    }
}

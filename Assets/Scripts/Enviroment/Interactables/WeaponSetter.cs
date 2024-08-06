using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSetter : RarityInteractable
{
    public GameObject weaponObject;

    float[] rarityDamagesAdded = { 0, .5f, 1.5f, 2.5f, 4 };

    Weapon weapon;

    int randomWeaponElement = 0;
    int randomWeaponType = 0;

    public override void Start()
    {
        base.Start();

        weapon = GetWeapon();

        while (weapon.attackSpriteSheet.Length <= 0)
        {
            weapon = GetWeapon();
        }
    }

    Weapon GetWeapon()
    {
        randomWeaponElement = Random.Range(0, System.Enum.GetValues(typeof(WeaponManager.Element)).Length);
        randomWeaponType = Random.Range(0, System.Enum.GetValues(typeof(WeaponManager.Type)).Length);

        weaponObject = WeaponManager.instance.LoadWeapon((WeaponManager.Element)randomWeaponElement, (WeaponManager.Type)randomWeaponType);

        if (weaponObject.TryGetComponent(out Weapon weapon))
        {
            weapon.weaponAddedDamage = rarityDamagesAdded[(int)rarity];
            displayImage.sprite = weapon.weaponSprite;
            displayText.text = weapon.weaponName;

            return weapon;
        }

        return null;
    }

    public override void Interact()
    {
        base.Interact();

        _animator.SetBool("IsInRange", false);

        if (weapon.attackSpriteSheet.Length <= 0) throw new System.Exception("Weapon does not have a attack animation");

        WeaponManager.instance.SetWeapon(weaponObject, PlayerController.instance.transform);
    }
}

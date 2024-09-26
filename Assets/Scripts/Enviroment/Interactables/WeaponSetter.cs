using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSetter : RarityInteractable, IDataPersistence
{
    [Space]

    public TextMeshProUGUI addedDamageText;
    public TextMeshProUGUI addedAttackSpeedText;

    [Space]

    public Image effectImage;
    public TextMeshProUGUI effectText;

    float[] rarityDamagesAdded = { 0, .5f, 1.5f, 2.5f, 4 };
    float[] rarityAttackSpeedAdded = { 0, -.05f, -.1f, -.2f, -.35f };

    GameObject weaponObject;
    Weapon weapon;

    int randomWeaponElement = 0;
    int randomWeaponType = 0;

    public override void Start()
    {
        base.Start();

        StartCoroutine(GetUsebleWeapon(.1f));
    }

    IEnumerator GetUsebleWeapon(float delay)
    {
        yield return new WaitForSeconds(delay);

        weapon = GetWeapon();

        while (weapon.attackSpriteSheet.Length <= 0)
        {
            weapon = GetWeapon();
        }
    }

    Weapon GetWeapon()
    {
        randomWeaponElement = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponManager.Element)).Length);
        randomWeaponType = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponManager.Type)).Length);

        weaponObject = WeaponManager.instance.LoadWeapon((WeaponManager.Element)randomWeaponElement, (WeaponManager.Type)randomWeaponType);

        if (weaponObject.TryGetComponent(out Weapon weapon))
        {
            weapon.weaponAddedDamage += rarityDamagesAdded[(int)rarity];
            weapon.weaponAddedAttackSpeed += rarityAttackSpeedAdded[(int)rarity];

            displayImage.sprite = weapon.weaponSprite;
            displayText.text = "" + weapon.weaponName;

            addedDamageText.text = "Weapon Damage: " + (weapon.weaponAddedDamage >= PlayerController.instance.currentWeapon.weaponAddedDamage ?
                "<color=green>" : "<color=red>") + Math.Round(weapon.weaponAddedDamage, 2) + "</color>" + " [" + Math.Round(PlayerController.instance.currentWeapon.weaponAddedDamage, 2) + "]";
            addedAttackSpeedText.text = "Weapon Attack Speed: " + (weapon.weaponAddedAttackSpeed <= PlayerController.instance.currentWeapon.weaponAddedAttackSpeed ?
                "<color=green>" : "<color=red>") + Math.Round(weapon.weaponAddedAttackSpeed, 2) + "</color>" + " [" + Math.Round(PlayerController.instance.currentWeapon.weaponAddedAttackSpeed, 2) + "]";

            if (weaponObject.TryGetComponent(out EffectManager effectManager))
            {
                if (effectManager.appliesEffects)
                {
                    effectManager.parameters.power = (int)rarity + 2;

                    effectImage.transform.parent.gameObject.SetActive(true);
                    effectText.gameObject.SetActive(true);

                    effectImage.sprite = PopUp.instance.effectSprites[(int)effectManager.parameters.type];
                    effectText.text = "Weapon Effect: " + effectManager.parameters.type.ToString();
                }
                else
                {
                    effectImage.transform.parent.gameObject.SetActive(false);
                    effectText.gameObject.SetActive(false);
                }
            }
            

            return weapon;
        }

        return null;
    }

    public override void Interact()
    {
        base.Interact();

        if (hasUses) animator.SetBool("IsInRange", false);

        if (weapon.attackSpriteSheet.Length <= 0) throw new System.Exception("Weapon does not have a attack animation");

        WeaponManager.instance.SetWeapon(weaponObject, PlayerController.instance.transform);

        if (!hasUses)
        {
            SetRarity();
            StartCoroutine(GetUsebleWeapon(.1f));
        }

        StartCoroutine(UpdateUIFromHUD());

        Weapon currentWeapon = weaponObject.GetComponent<Weapon>();

        DataPersistenceManager.instance.gameData.weaponElement = currentWeapon.weaponElement;
        DataPersistenceManager.instance.gameData.weaponType = currentWeapon.weaponType;
        DataPersistenceManager.instance.gameData.weaponAddedDamage = currentWeapon.weaponAddedDamage;
        DataPersistenceManager.instance.gameData.weaponAddedAttackSpeed = currentWeapon.weaponAddedAttackSpeed;
        DataPersistenceManager.instance.gameData.effectPower = currentWeapon.gameObject.GetComponent<EffectManager>().parameters.power;
    }

    IEnumerator UpdateUIFromHUD()
    {
        yield return new WaitForSeconds(.15f);

        HudManager.instance.UpdateStatsUI();
    }

    string RarityLevelTextDisplay(Rarity rarity)
    {
        string posDisplay = "O";
        string negDisplay = " I";
        string finalDisplay = "";

        for (int i = 0; i < System.Enum.GetValues(typeof(Rarity)).Length; i++)
        {
            finalDisplay += (i <= (int)rarity) ? posDisplay : negDisplay;
        }

        return finalDisplay;
    }

    public void LoadData(GameData data)
    {
        throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {

        
    }
}

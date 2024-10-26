using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSetter : RarityInteractable
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

        GetWeapon();
    }

    void GetWeapon()
    {
        SetRarity();

        randomWeaponElement = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponManager.Element)).Length);
        randomWeaponType = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponManager.Type)).Length);

        // Get saved data
        randomWeaponElement = InteractableManager.instance.GetInteractableValue(id, 1, randomWeaponElement);
        randomWeaponType = InteractableManager.instance.GetInteractableValue(id, 2, randomWeaponType);

        weaponObject = WeaponManager.instance.LoadWeapon((WeaponManager.Element)randomWeaponElement, (WeaponManager.Type)randomWeaponType);

        if (weaponObject.TryGetComponent(out Weapon weapon))
        {
            weapon.weaponAddedDamage += rarityDamagesAdded[(int)rarity];
            weapon.weaponAddedAttackSpeed += rarityAttackSpeedAdded[(int)rarity];
            #region UI_Dysplay
            displayImage.sprite = weapon.weaponSprite;

            displayText.text = "" + weapon.weaponName;

            addedDamageText.text = "DMG: " 
                + (weapon.weaponAddedDamage >= PlayerController.instance.currentWeapon.weaponAddedDamage ?
                "<color=green>" : "<color=red>") 
                + (weapon.weaponAddedDamage > 0 ? " + " : (weapon.weaponAddedDamage == 0 ? " " : " - ")) +
                + Math.Round(Mathf.Abs(weapon.weaponAddedDamage), 2) 
                + "</color>" 
                + " [" + Math.Round(PlayerController.instance.currentWeapon.weaponAddedDamage, 2) + "]";

            addedAttackSpeedText.text = "ATK SPD: "
                + (weapon.weaponAddedAttackSpeed <= PlayerController.instance.currentWeapon.weaponAddedAttackSpeed ?
                "<color=green>" : "<color=red>") 
                + (weapon.weaponAddedAttackSpeed > 0 ? " + " : (weapon.weaponAddedAttackSpeed == 0 ? " " : " - ")) +
                + Math.Round(Mathf.Abs(weapon.weaponAddedAttackSpeed), 2) 
                + "</color>" 
                + " [" + Math.Round(PlayerController.instance.currentWeapon.weaponAddedAttackSpeed, 2) + "]";
            #endregion
            #region WeaponEffects
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
            #endregion
        }
    }

    public override void Interact()
    {
        base.Interact();

        if(weaponObject != null)
        {
            WeaponManager.instance.SetWeapon(weaponObject, PlayerController.instance.transform);

            StartCoroutine(UpdateStatsUI());

            Weapon currentWeapon = weaponObject.GetComponent<Weapon>();

            //Get Weapon Data
            #region WeaponData
            DataPersistenceManager.instance.gameData.weaponElement = currentWeapon.weaponElement;
            DataPersistenceManager.instance.gameData.weaponType = currentWeapon.weaponType;
            DataPersistenceManager.instance.gameData.weaponAddedDamage = currentWeapon.weaponAddedDamage;
            DataPersistenceManager.instance.gameData.weaponAddedAttackSpeed = currentWeapon.weaponAddedAttackSpeed;
            DataPersistenceManager.instance.gameData.effectPower = currentWeapon.gameObject.GetComponent<EffectManager>().parameters.power;
            #endregion

            InteractableManager.instance.SaveInteractableStructure(this);

            if (hasUses) uses--;
            else InteractableManager.instance.UnsaveInteractableStructure(this);
        }

        if (!hasUses)
        {
            GetWeapon();
            SaveData();
        }
        else animator.SetBool("IsInRange", false);
    }

    IEnumerator UpdateStatsUI()
    {
        yield return null;
        HudManager.instance.UpdateStatsUI();
    }

    public override void SaveData()
    {
        base.SaveData();

        InteractableManager.instance.SetInteractableValues(id, (int)rarity, 0);
        InteractableManager.instance.SetInteractableValues(id, randomWeaponElement, 1);
        InteractableManager.instance.SetInteractableValues(id, randomWeaponType, 2);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        // Save data
        SaveData();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatUpgrader : RarityInteractable
{
    [SerializeField] Sprite[] statSprites;

    int randomStatIndex = 0;

    float[] rarityMuliplier = { 1, 1.5f, 2.5f, 4, 6 };

    Entity.Stat randomStat;

    public override void Start()
    {
        base.Start();

        GetStat();
    }

    void GetStat()
    {
        SetRarity();

        randomStatIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Entity.Stat)).Length);

        // Get saved data
        randomStatIndex = InteractableManager.instance.GetInteractableValue(id, 1, randomStatIndex);

        randomStat = (Entity.Stat)randomStatIndex;

        if (!PlayerController.instance.CanSetStat(randomStat))
        {
            displayImage.sprite = statSprites[randomStatIndex];
            displayText.text = "Upgraded To Max";

            return;
        }

        DisplayStat();
    }

    void DisplayStat()
    {
        displayImage.sprite = statSprites[randomStatIndex];
        displayText.text = "" + randomStat.ToString().ToUpper() + ":\n" + Math.Round(PlayerController.instance.GetStat(randomStat), 2);
        displayText.text += "<color=green>" + (PlayerController.instance.GetStatMultiplier(randomStat) > 0 ? " + " : " - ") +
            Math.Round(Mathf.Abs(PlayerController.instance.GetStatMultiplier(randomStat) * rarityMuliplier[(int)rarity]), 2) + "</color>";
    }

    public override void Interact()
    {
        base.Interact();

        if (PlayerController.instance.CanSetStat(randomStat))
        {
            PlayerController.instance.SetStat(randomStat, PlayerController.instance.GetStatMultiplier(randomStat) * rarityMuliplier[(int)rarity], true);
            HudManager.instance.UpdateStatsUI();

            InteractableManager.instance.SaveInteractableStructure(this);

            if (hasUses)
                uses--;
            else
                InteractableManager.instance.UnsaveInteractableStructure(this);
        }

        if (!hasUses)
        {
            GetStat();
            SaveData();
        }
        else
            animator.SetBool("IsInRange", false);
    }

    public override void SaveData()
    {
        base.SaveData();

        InteractableManager.instance.SetInteractableValues(id, (int)rarity, 0);
        InteractableManager.instance.SetInteractableValues(id, randomStatIndex, 1);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        DisplayStat();

        // Save data
        SaveData();
    }
}

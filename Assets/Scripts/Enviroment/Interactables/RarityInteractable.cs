using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RarityInteractable;

public class RarityInteractable : Interactable
{
    public enum Rarity
    {
        common, uncommon, rare, epic, legendary
    };

    protected Rarity rarity;

    [Space]

    public Image rarityImage;
    public TextMeshProUGUI rarityText;

    [Space]

    public Image displayImage;
    public TextMeshProUGUI displayText;

    [Space]

    public Color[] rarityColors;

    public virtual void Start()
    {
        SetRarity();
    }

    protected void SetRarity()
    {
        float randomValue = Random.value;

        if (randomValue > 0 && randomValue < .6f)
        {
            rarity = Rarity.common;
        }
        else if (randomValue > .6f && randomValue < .90f)
        {
            rarity = Rarity.uncommon;
        }
        else if (randomValue > .90f && randomValue < .95f)
        {
            rarity = Rarity.rare;
        }
        else if (randomValue > .95f && randomValue < .98f)
        {
            rarity = Rarity.epic;
        }
        else
        {
            rarity = Rarity.legendary;
        }

        rarityImage.color = rarityColors[(int)rarity];
        rarityText.color = rarityColors[(int)rarity];
        rarityText.text = rarity.ToString();
    }
}

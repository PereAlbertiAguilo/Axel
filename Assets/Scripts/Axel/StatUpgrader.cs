using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



// TODO: UI shopw rarity with a panel and stat upgrade amount ---------------------
//                                                           |   rarity:           |
//                                                           |   current + amount  |
//                                                           |      sprite ?       |
//                                                           |                     |
//                                                            ---------------------


public class StatUpgrader : MonoBehaviour
{
    [SerializeField] Image rarityImage;
    [SerializeField] Image statImage;
    [SerializeField] TextMeshProUGUI rarityText;
    [SerializeField] TextMeshProUGUI statAmountText;

    Animator _animator;

    [SerializeField] Color[] rarityColors; 
    [SerializeField] Sprite[] statsSprites;

    bool hasInteracted = false;

    bool canInteract = false;

    public int randomStat = 0;

    public enum Rarity
    {
        common, uncommon, rare, epic, legendary
    };

    public Rarity rarity;

    public float rarityMuliplier = 1f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rarity = SetRarity();

        randomStat = Random.Range(-1, 7) + 1;

        statImage.sprite = statsSprites[randomStat];
        statAmountText.text = "" + GetStatToUpgrade(randomStat) +  ": " + StatsManager.instance.stats[randomStat].statValue;
        statAmountText.text += (StatsManager.instance.stats[randomStat].statMultiliper > 0 ? " + " : " - ") + 
            Mathf.Abs(StatsManager.instance.stats[randomStat].statMultiliper) * rarityMuliplier;
    }

    Rarity SetRarity()
    {
        float randomValue = Random.value;
        Rarity rarity = Rarity.common;

        if(randomValue > 0 && randomValue < .6f)
        {
            rarityMuliplier = 1f;
            rarity = Rarity.common;
        }
        else if (randomValue > .6f && randomValue < .90f)
        {
            rarityMuliplier = 1.2f;
            rarity = Rarity.uncommon;
        }
        else if(randomValue > .90f && randomValue < .95f)
        {
            rarityMuliplier = 1.8f;
            rarity = Rarity.rare;
        }
        else if (randomValue > .95f && randomValue < .98f)
        {
            rarityMuliplier = 2.5f;
            rarity = Rarity.epic;
        }
        else
        {
            rarityMuliplier = 4f;
            rarity = Rarity.legendary;
        }

        rarityImage.color = rarityColors[(int)rarity];
        rarityText.color = rarityColors[(int)rarity];
        rarityText.text = rarity.ToString();

        return rarity;
    }

    private void Update()
    {
        if(UserInput.instance.interactInput && canInteract && !hasInteracted)
        {
            hasInteracted = true;
            StatsManager.instance.UpgradeStat(GetStatToUpgrade(randomStat), StatsManager.instance.stats[randomStat].statMultiliper * rarityMuliplier);
        }
    }

    string GetStatToUpgrade(int statIndex)
    {
        return StatsManager.instance.stats[statIndex].name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            _animator.SetBool("IsInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            _animator.SetBool("IsInRange", false);
        }
    }
}

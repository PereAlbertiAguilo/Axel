using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public int randomStatIndex = 0;

    public enum Rarity
    {
        common, uncommon, rare, epic, legendary
    };

    public Rarity rarity;

    float[] rarityMuliplier = { 1, 1.5f, 2.5f, 4, 6 };

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetRarity();

        randomStatIndex = Random.Range(0, 3);

        statImage.sprite = statsSprites[randomStatIndex];
        //statAmountText.text = "" + StatFromIndex(randomStatIndex).name +  ":\n" + StatFromIndex(randomStatIndex).statValue;
        //statAmountText.text += "<color=green>" + (StatFromIndex(randomStatIndex).statMultiplier > 0 ? " + " : " - ") +
        //    Mathf.Abs(StatFromIndex(randomStatIndex).statMultiplier * rarityMuliplier[(int)rarity]) + "</color>";
    }

    private void Update()
    {
        if (UserInput.instance.interactInput && canInteract && !hasInteracted /*&& StatsManager.instance.canModifyStats*/)
        {
            hasInteracted = true;
            //StatsManager.instance.UpgradeStat(StatFromIndex(randomStatIndex), StatFromIndex(randomStatIndex).statMultiplier * rarityMuliplier[(int)rarity]);
            _animator.SetBool("IsInRange", false);
        }
    }

    void SetRarity()
    {
        float randomValue = Random.value;

        if(randomValue > 0 && randomValue < .6f)
        {
            //rarityMuliplier = 1f;
            rarity = Rarity.common;
        }
        else if (randomValue > .6f && randomValue < .90f)
        {
            //rarityMuliplier = 1.2f;
            rarity = Rarity.uncommon;
        }
        else if(randomValue > .90f && randomValue < .95f)
        {
            //rarityMuliplier = 1.8f;
            rarity = Rarity.rare;
        }
        else if (randomValue > .95f && randomValue < .98f)
        {
            //rarityMuliplier = 2.5f;
            rarity = Rarity.epic;
        }
        else
        {
            //rarityMuliplier = 4f;
            rarity = Rarity.legendary;
        }

        rarityImage.color = rarityColors[(int)rarity];
        rarityText.color = rarityColors[(int)rarity];
        rarityText.text = rarity.ToString();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasInteracted)
        {
            canInteract = true;
            _animator.SetBool("IsInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasInteracted)
        {
            canInteract = false;
            _animator.SetBool("IsInRange", false);
        }
    }
}

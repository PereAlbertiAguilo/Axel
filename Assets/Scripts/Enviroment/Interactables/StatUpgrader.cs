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
    [SerializeField] TextMeshProUGUI statText;

    Animator _animator;

    [SerializeField] Color[] rarityColors; 
    [SerializeField] Sprite[] statSprites;

    bool hasInteracted = false;

    bool canInteract = false;

    public int randomStatIndex = 0;

    public enum Rarity
    {
        common, uncommon, rare, epic, legendary
    };

    public Rarity rarity;

    float[] rarityMuliplier = { 1, 1.5f, 2.5f, 4, 6 };

    Entity.Stat randomStat;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetRarity();

        randomStatIndex = Random.Range(0, 4);
        randomStat = (Entity.Stat)randomStatIndex;

        statImage.sprite = statSprites[randomStatIndex];
        statText.text = "" + randomStat.ToString().ToUpper() +  ":\n" + PlayerController.instance.GetStat(randomStat);
        statText.text += "<color=green>" + (PlayerController.instance.GetStatMultiplier(randomStat) > 0 ? " + " : " - ") +
            Mathf.Abs(PlayerController.instance.GetStatMultiplier(randomStat) * rarityMuliplier[(int)rarity]) + "</color>";
    }

    private void Update()
    {
        if (UserInput.instance.interactInput && canInteract && !hasInteracted)
        {
            hasInteracted = true;
            PlayerController.instance.SetStat(randomStat, PlayerController.instance.GetStatMultiplier(randomStat) * rarityMuliplier[(int)rarity]);
            HudManager.instance.UpdateStatsUI();
            _animator.SetBool("IsInRange", false);
        }
    }

    void SetRarity()
    {
        float randomValue = Random.value;

        if(randomValue > 0 && randomValue < .6f)
        {
            rarity = Rarity.common;
        }
        else if (randomValue > .6f && randomValue < .90f)
        {
            rarity = Rarity.uncommon;
        }
        else if(randomValue > .90f && randomValue < .95f)
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

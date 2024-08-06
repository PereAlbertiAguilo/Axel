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

        randomStatIndex = Random.Range(0, System.Enum.GetValues(typeof(Entity.Stat)).Length);
        randomStat = (Entity.Stat)randomStatIndex;

        displayImage.sprite = statSprites[randomStatIndex];
        displayText.text = "" + randomStat.ToString().ToUpper() +  ":\n" + PlayerController.instance.GetStat(randomStat);
        displayText.text += "<color=green>" + (PlayerController.instance.GetStatMultiplier(randomStat) > 0 ? " + " : " - ") +
            Mathf.Abs(PlayerController.instance.GetStatMultiplier(randomStat) * rarityMuliplier[(int)rarity]) + "</color>";
    }

    public override void Interact()
    {
        base.Interact();

        PlayerController.instance.SetStat(randomStat, PlayerController.instance.GetStatMultiplier(randomStat) * rarityMuliplier[(int)rarity]);
        HudManager.instance.UpdateStatsUI();
        _animator.SetBool("IsInRange", false);
    }
}

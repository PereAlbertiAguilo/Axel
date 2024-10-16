using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO: Upgrading system. Items with rareity ? maybe ? idk... / treasure rooms ? / purchasable ? ] 
// TODO: Money / Cins / Currency / whatever... (brainstorming)                                <---]

public class HudManager : MonoBehaviour
{
    [SerializeField] Image dashCooldownImage;
    [SerializeField] Image healthBarImage;
    [SerializeField] Image delayedHealthBarImage;

    [Space]

    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject statsLayout;
    [SerializeField] GameObject colectablesLayout;

    [Space]

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI currentFloorText;
    [SerializeField] TextMeshProUGUI currentFloorInitialText;
    [SerializeField] TextMeshProUGUI currentHealthText;

    [Space]

    public float timer = 0;

    public bool keepStatsUp = false;

    public static HudManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke(nameof(UpdateStatsUI), .1f);

        currentFloorText.text = FloorName(true);
        currentFloorInitialText.text = FloorName(false);

        timer = PlayerPrefs.HasKey("timer") ? PlayerPrefs.GetFloat("timer") : 0;
        keepStatsUp = PlayerPrefs.HasKey("stats") ? ((PlayerPrefs.GetInt("stats") > 0) ? true : false) : false;
        statsPanel.SetActive(keepStatsUp);
    }

    private void Update()
    {
        if (!PauseMenu.instance.isPaused && !GameManager.instance.isGameOver)
        {
            if (UserInput.instance.statsInputDown)
            {
                keepStatsUp = false;

                if (!statsPanel.activeInHierarchy)
                {
                    StartCoroutine(KeepStatsUp());

                    statsPanel.SetActive(true);
                }
            }
            else if (UserInput.instance.statsInputUp && !keepStatsUp)
            {
                statsPanel.SetActive(false);
            }
        }

        if (!PauseMenu.instance.isPaused && !GameManager.instance.isGameOver) timer += Time.deltaTime;

        timerText.text = Timer();

        currentHealthText.text = PlayerController.instance.healthCurrent.ToString("0.0");

        healthBarImage.fillAmount = PlayerController.instance.healthCurrent / PlayerController.instance.health;

        delayedHealthBarImage.fillAmount = Mathf.Lerp(delayedHealthBarImage.fillAmount, PlayerController.instance.healthCurrent / PlayerController.instance.health, Time.deltaTime / .5f);
    }


    public string Timer()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        return string.Format("{00:0}:{1:00}", minutes, seconds);
    }

    public string FloorName(bool toUppert)
    {
        string finalName = "";

        foreach (char character in SceneManager.GetActiveScene().name)
        {
            if (char.IsUpper(character)) finalName += " ";
            finalName += character;
        }

        if (toUppert) finalName = finalName.ToUpper();

        return finalName;
    }

    public IEnumerator DashCooldownBar(float duration)
    {
        float currentTime = 0;

        dashCooldownImage.fillAmount = 0;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;

            dashCooldownImage.fillAmount = Mathf.Lerp(0, 1, currentTime / duration);

            yield return null;
        }

        dashCooldownImage.fillAmount = 1;
    }

    public IEnumerator HealthBar(float duration)
    {
        float currentTime = 0;

        healthBarImage.fillAmount = PlayerController.instance.healthCurrent / PlayerController.instance.health;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;

            delayedHealthBarImage.fillAmount = Mathf.Lerp(delayedHealthBarImage.fillAmount, 
                PlayerController.instance.healthCurrent / PlayerController.instance.health, currentTime / duration);

            yield return null;
        }

        //delayedHealthBarImage.fillAmount = PlayerController.instance.healthCurrent / PlayerController.instance.health;
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Entity.Stat)).Length; i++)
        {
            TextMeshProUGUI currentStatText = statsLayout.transform.GetChild(i).Find("StatInfo").GetComponent<TextMeshProUGUI>();
            string statText = "" + Math.Round(PlayerController.instance.GetStat((Entity.Stat)i), 2);

            currentStatText.text = statText;

            if (i == (int)Entity.Stat.damage) currentStatText.text = statText + " [" + 
                    (PlayerController.instance.currentWeapon.weaponAddedDamage > 0 ? 
                    "<color=green>" : "<color=red>") + Math.Round(PlayerController.instance.currentWeapon.weaponAddedDamage, 2) + "</color>" + "]";
            if (i == (int)Entity.Stat.attackSpeed) currentStatText.text = statText + " [" +
                    (PlayerController.instance.currentWeapon.weaponAddedAttackSpeed < 0 ?
                    "<color=green>" : "<color=red>") + Math.Round(PlayerController.instance.currentWeapon.weaponAddedAttackSpeed, 2) + "</color>" + "]";
        }
    }

    public void UpdateCollectableUI()
    {
        for (int i = 0; i < Enum.GetValues(typeof(CollectiblesManager.Types)).Length; i++)
        {
            TextMeshProUGUI currentStatText = colectablesLayout.transform.GetChild(i).Find("Amount").GetComponent<TextMeshProUGUI>();
            string statText = "" + CollectiblesManager.instance.GetCollectable((CollectiblesManager.Types)i).GetValue(CollectiblesManager.instance);

            currentStatText.text = statText;
        }
    }

    IEnumerator KeepStatsUp()
    {
        keepStatsUp = true;

        yield return new WaitForSeconds(.2f);

        if (UserInput.instance.statsInput) keepStatsUp = false;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("timer", timer);
        PlayerPrefs.SetInt("stats", keepStatsUp ? 1 : 0);
    }
}

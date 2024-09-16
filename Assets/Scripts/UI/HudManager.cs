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

    [Space]

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI currentHealthText;

    float timer = 0;

    public static HudManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke(nameof(UpdateStatsUI), .1f);

        stageText.text = StageName();
    }

    private void Update()
    {
        if (!PauseMenu.instance.paused)
        {
            if (UserInput.instance.statsInputDown)
            {
                statsPanel.SetActive(true);
            }
            else if (UserInput.instance.statsInputUp)
            {
                statsPanel.SetActive(false);
            }
        }

        if (!PauseMenu.instance.paused) Timer();

        currentHealthText.text = PlayerController.instance.healthCurrent.ToString("0.0");

        healthBarImage.fillAmount = PlayerController.instance.healthCurrent / PlayerController.instance.health;

        delayedHealthBarImage.fillAmount = Mathf.Lerp(delayedHealthBarImage.fillAmount, PlayerController.instance.healthCurrent / PlayerController.instance.health, Time.deltaTime / .5f);
    }

    void Timer()
    {
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        timerText.text = string.Format("{00:0}:{1:00}", minutes, seconds);
    }

    string StageName()
    {
        string finalName = "";

        foreach (char character in SceneManager.GetActiveScene().name)
        {
            if (char.IsUpper(character)) finalName += " ";
            finalName += character;
        }

        finalName = finalName.ToUpper();

        return finalName;
    }

    public IEnumerator DashCooldownBar(float dashCooldown)
    {
        dashCooldownImage.fillAmount = 0;

        while (dashCooldownImage.fillAmount < 1)
        {
            dashCooldownImage.fillAmount += Time.deltaTime / dashCooldown;

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

            delayedHealthBarImage.fillAmount = Mathf.Lerp(delayedHealthBarImage.fillAmount, PlayerController.instance.healthCurrent / PlayerController.instance.health, currentTime / duration);

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
                    "<color=green>" : "<color=red>") + PlayerController.instance.currentWeapon.weaponAddedDamage + "</color>" + "]";
            if (i == (int)Entity.Stat.attackSpeed) currentStatText.text = statText + " [" +
                    (PlayerController.instance.currentWeapon.weaponAddedAttackSpeed < 0 ?
                    "<color=green>" : "<color=red>") + PlayerController.instance.currentWeapon.weaponAddedAttackSpeed + "</color>" + "]";
        }
    }
}

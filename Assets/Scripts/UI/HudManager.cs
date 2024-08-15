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

    [Space]

    [SerializeField] GameObject miniMap;
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

        stageText.text = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (!PauseMenu.instance.paused)
        {
            if (UserInput.instance.statsInputDown)
            {
                miniMap.SetActive(false);
                statsPanel.SetActive(true);
            }
            else if (UserInput.instance.statsInputUp)
            {
                miniMap.SetActive(true);
                statsPanel.SetActive(false);
            }
        }

        if (!PauseMenu.instance.paused) Timer();

        currentHealthText.text = "" + Math.Round(PlayerController.instance.healthCurrent, 1);

        healthBarImage.fillAmount = PlayerController.instance.healthCurrent / PlayerController.instance.health;
    }

    void Timer()
    {
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        timerText.text = string.Format("{00:0}:{1:00}", minutes, seconds);
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: Upgrading system. Items with rareity ? maybe ? idk... / treasure rooms ? / purchasable ? ] 
// TODO: Time / StageName                                                                         |
// TODO: Money / Cins / Currency / whatever... (brainstorming)                                <---]


public class HudManager : MonoBehaviour
{
    [SerializeField] Image dashCooldownImage;
    public GameObject ammoBar;
    [SerializeField] GameObject ammoPoint;

    [SerializeField] GameObject topBar;
    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject statsLayout;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI currentHealthText;
    float timer = 0;

    public static HudManager instance;
    PlayerAttack playerAttack;
    PlayerController playerController;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerController = FindObjectOfType<PlayerController>();

        UpdateStatsUI();
    }

    private void Update()
    {
        if (!PauseMenu.instance.paused)
        {
            if (UserInput.instance.statsInputDown)
            {
                topBar.SetActive(false);
                statsPanel.SetActive(true);
            }
            else if (UserInput.instance.statsInputUp)
            {
                topBar.SetActive(true);
                statsPanel.SetActive(false);
            }
        }

        if (!PauseMenu.instance.paused) Timer();

        currentHealthText.text = "" + Math.Round(playerController._playerHealth.currentHealth, 2);
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

    public IEnumerator FillAmmoBar(int ammoToFill, float reloadTime)
    {
        for (int i = 0; i < ammoToFill; i++)
        {
            Instantiate(ammoPoint, ammoBar.transform);
            yield return new WaitForSeconds((reloadTime / ammoToFill) / 2);
        }
    }

    public void EmptyAmmoBar()
    {
        foreach (Transform ammoPoint in ammoBar.transform)
        {
            Destroy(ammoPoint.gameObject);
        }
    }

    public void UpdateAmmoBar(int currentAmmo)
    {
        if(currentAmmo > 0)
        {
            Destroy(ammoBar.transform.GetChild(ammoBar.transform.childCount - 1).gameObject);
        }
    }

    public void UpdateStatsUI()
    {
        StatsManager sm = StatsManager.instance;

        int index = 0;

        foreach (Transform stat in statsLayout.transform)
        {
            stat.GetChild(2).GetComponent<TextMeshProUGUI>().text = "" + Math.Round(sm.stats[index].statValue, 2);
            index++;
        }

        EmptyAmmoBar();
        StartCoroutine(FillAmmoBar(Mathf.CeilToInt(StatsManager.instance.GetStat(StatsManager.StatType.ammo).statValue), playerAttack.throwAttackReloadTime));
    }
}

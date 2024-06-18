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
    float timer = 0;

    public static HudManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
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

    public void FillAmmoBar(int ammoToFill)
    {
        for (int i = 0; i < ammoToFill; i++)
        {
            Instantiate(ammoPoint, ammoBar.transform);
        }
    }

    public void EmptyAmmoBar()
    {
        foreach (Transform ammoPoint in ammoBar.transform)
        {
            Destroy(ammoPoint.gameObject);
        }
    }

    public IEnumerator ReloadAmmoBar(int ammoToFill, float reloadTime)
    {
        for (int i = 0; i < ammoToFill; i++)
        {
            Instantiate(ammoPoint, ammoBar.transform);
            yield return new WaitForSeconds((reloadTime / ammoToFill) / 2);
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
            stat.GetChild(2).GetComponent<TextMeshProUGUI>().text = "" + sm.stats[index].statValue;
            index++;
        }

        EmptyAmmoBar();
        FillAmmoBar((int)sm.stats[4].statValue);
    }
}

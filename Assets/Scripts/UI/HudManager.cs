using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// TODO: Stats manager 
// TODO: Time / StageName
// TODO: Link stats to UI
// TODO: Money / Cins / Currency / whatever... (brainstorming)


public class HudManager : MonoBehaviour
{
    [SerializeField] Image dashCooldownImage;
    public GameObject ammoBar;
    [SerializeField] GameObject ammoPoint;

    [SerializeField] GameObject topBar;
    [SerializeField] GameObject statsPanel;

    public static HudManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
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

    public IEnumerator ReloadAmmoBar(int ammoToFill, float reloadTime)
    {
        for (int i = 0; i < ammoToFill; i++)
        {
            yield return new WaitForSeconds((reloadTime / ammoToFill) / 2);
            Instantiate(ammoPoint, ammoBar.transform);
        }
    }

    public void UpdateAmmoBar(int currentAmmo)
    {
        if(currentAmmo > 0)
        {
            Destroy(ammoBar.transform.GetChild(ammoBar.transform.childCount - 1).gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] Image dashCooldownImage;
    [SerializeField] GameObject ammoBar;
    [SerializeField] GameObject ammoPoint;

    public static HudManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
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

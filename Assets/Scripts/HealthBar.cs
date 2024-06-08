using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;

    public IEnumerator UpdateHealthBar(float currentHealth, float maxHealth, float duration)
    {
        float startFillAmount = healthBar.fillAmount;
        float endFillAmount = currentHealth / maxHealth;

        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            healthBar.fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, currentTime / duration);

            yield return null;
        }

        healthBar.fillAmount = endFillAmount;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    PlayerController playerController;

    public static StatsManager instance;

    [Serializable]
    public class Stat
    {
        public string name;
        public float statValue;
        public float[] statMultiliper;
    }

    public List<Stat> stats = new List<Stat>();

    private void Awake()
    {
        instance = this;

        playerController = FindObjectOfType<PlayerController>();
    }

    public void UpgradeStat(Stat statToUpgrade, float statChange)
    {
        statToUpgrade.statValue += statChange;

        playerController._playerHealth.maxHealth = stats[0].statValue;
        playerController._playerHealth.healthBar.StartCoroutine(playerController._playerHealth.healthBar.UpdateHealthBar(playerController._playerHealth.currentHealth, StatsManager.instance.stats[0].statValue, .4f));

        HudManager.instance.UpdateStatsUI();
    }
}

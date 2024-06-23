using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    PlayerController playerController;

    public static StatsManager instance;

    public enum StatType
    {
        health, nDamage, tDamage, aRate, ammo, speed, dSpeed, dCooldown
    }

    [Serializable]
    public class Stat
    {
        public string name;
        public float statValue;
        public float statMultiplier;
        public StatType statType;
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

        playerController._playerHealth.maxHealth = GetStat(StatType.health).statValue;
        playerController._playerHealth.healthBar.StartCoroutine(playerController._playerHealth.healthBar.UpdateHealthBar(playerController._playerHealth.currentHealth, 
            GetStat(StatType.health).statValue, .4f));

        HudManager.instance.UpdateStatsUI();
    }

    public Stat GetStat(StatType statType)
    {
        return stats.Find(x => x.statType == statType);
    }
}

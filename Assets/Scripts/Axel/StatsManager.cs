using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    [Serializable]
    public class Stat
    {
        public string name;
        public float statValue;
        public float statMultiliper;
    }

    public List<Stat> stats = new List<Stat>();

    private void Awake()
    {
        instance = this;
    }

    public void UpgradeStat(string statName, float statChange)
    {
        stats.Find(x => x.name == statName).statValue += statChange;

        HudManager.instance.UpdateStatsUI();
    }
}

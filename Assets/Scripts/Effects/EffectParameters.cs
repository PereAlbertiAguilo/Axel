using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectParameters
{
    public enum Type
    {
        Slowness, Weakness, Stun, Dot, DefenseBreak
    };

    public Type type;
    [Space]
    public float duration = 1;
    [HideInInspector] public float durationCurrent = 1;
    [Range(1f, 10f), Space]
    public int power = 1;
    [Space]
    public float cooldown;
}

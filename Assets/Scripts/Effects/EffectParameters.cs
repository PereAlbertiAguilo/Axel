using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectParameters
{
    public enum Type
    {
        Slowness, Weakness, Stun, Dot, DefenseBreak, Knockback
    };

    public Type type;
    [Space]
    public float duration = 1;
    [HideInInspector] public float durationCurrent = 1;
    [Range(2f, 6f),Min(2), Space]
    public int power = 1;
    [Space]
    public float cooldown;
}

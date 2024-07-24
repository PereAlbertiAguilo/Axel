using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectParameters
{
    public string name;

    public enum Type
    {
        Slowness, Weakness, Stun, Dot
    };

    public Type type;
    [Space]
    public float duration = 1;
    [Range(1f, 10f), Space]
    public int power = 1;
}

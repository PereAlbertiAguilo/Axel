using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Effect : MonoBehaviour
{
    public enum EffectType
    {
        Slowness, Weakness, Stun, Dot
    };

    public EffectType type;
    [Space]
    public float effectDuration;
    [Range(1f, 10f), Space]
    public int effectPower;
    public bool effectIsActive = false;

    [HideInInspector] public float currentTime;

    [HideInInspector] public Entity entity;

    public void SetEffect()
    {
        currentTime = 0;
    }

    public virtual void EndEffect()
    {
    }

    protected float SetEffectPower(float value)
    {
        return effectPower * value / 10;
    }
}

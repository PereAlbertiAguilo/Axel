using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectParameters effectParameters;
    
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

    protected float SetEffectPower(float value, bool inverse)
    {
        return !inverse ? (effectParameters.power * value / 11) : (value / effectParameters.power);
    }
}

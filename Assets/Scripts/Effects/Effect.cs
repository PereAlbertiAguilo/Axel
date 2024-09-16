using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectParameters parameters;
    
    [HideInInspector] public float currentTime;

    [HideInInspector] public Entity entity;

    [HideInInspector] public Entity attakerEntity;

    public void SetEffect()
    {
        parameters.durationCurrent = parameters.duration;
        currentTime = 0;
    }

    public virtual void EndEffect()
    {
    }

    protected float SetEffectPower(float value, bool inverse)
    {
        return !inverse ? (parameters.power * value / 7) : (value / parameters.power);
    }
}

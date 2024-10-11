using System.Collections;
using UnityEngine;

public class Weakness : Effect
{
    public void Start()
    {
        SetEffect();
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            targetEntity.damageCurrent = SetEffectPower(targetEntity.damage, false);
        }
        else
        {
            EndEffect();

            Destroy(gameObject);
        }
    }

    public override void EndEffect()
    {
        currentTime = parameters.duration;
        targetEntity.damageCurrent = targetEntity.damage;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
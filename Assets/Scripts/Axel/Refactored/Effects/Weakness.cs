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
        if (currentTime < effectDuration)
        {
            currentTime += Time.deltaTime;

            entity.damageCurrent = SetEffectPower(entity.damage);
        }
        else
        {
            EndEffect();

            Destroy(gameObject);
        }
    }

    public override void EndEffect()
    {
        currentTime = effectDuration;
        entity.damageCurrent = entity.damage;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
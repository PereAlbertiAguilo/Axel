using System.Collections;
using UnityEngine;

public class Weakness : Effect
{
    public void Start()
    {
        SetEffect(entity.currentDamage);
    }

    private void Update()
    {
        if (currentTime < effectDuration)
        {
            currentTime += Time.deltaTime;

            entity.currentDamage = SetEffectPower(entity.damage);
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
        entity.currentDamage = entity.damage;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
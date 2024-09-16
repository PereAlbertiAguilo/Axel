using System.Collections;
using UnityEngine;

public class Slowness : Effect
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

            entity.speedCurrent = SetEffectPower(entity.speed, true);
            entity.attackSpeedCurrent = entity.attackSpeed * 2;
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
        entity.speedCurrent = entity.speed;
        entity.attackSpeedCurrent = entity.attackSpeed;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

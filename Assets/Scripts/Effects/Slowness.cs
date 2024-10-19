using System.Collections;
using UnityEngine;

public class Slowness : Effect
{
    SpriteAnimation spriteAnimation;

    float startValue;

    public void Start()
    {
        SetEffect();

        startValue = targetEntity.timeSpeed;
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            targetEntity.speedCurrent = SetEffectPower(targetEntity.speed, true);
            targetEntity.attackSpeedCurrent = targetEntity.attackSpeed * 1.5f;
            targetEntity.timeSpeed = startValue / 2;
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
        targetEntity.speedCurrent = targetEntity.speed;
        targetEntity.attackSpeedCurrent = targetEntity.attackSpeed;
        targetEntity.timeSpeed = startValue;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

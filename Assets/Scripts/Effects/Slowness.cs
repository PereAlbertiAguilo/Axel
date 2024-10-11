using System.Collections;
using UnityEngine;

public class Slowness : Effect
{
    SpriteAnimation spriteAnimation;

    float startValue;

    public void Start()
    {
        SetEffect();

        if (targetEntity.TryGetComponent(out spriteAnimation))
        {
            startValue = spriteAnimation.speed;
            spriteAnimation.speed *= 2;
        }
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            targetEntity.speedCurrent = SetEffectPower(targetEntity.speed, true);
            targetEntity.attackSpeedCurrent = targetEntity.attackSpeed * 1.5f;
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

        if (spriteAnimation != null) spriteAnimation.speed = startValue;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

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
        if (currentTime < effectDuration)
        {
            currentTime += Time.deltaTime;

            entity.speedCurrent = SetEffectPower(entity.speed);
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
        entity.speedCurrent = entity.speed;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

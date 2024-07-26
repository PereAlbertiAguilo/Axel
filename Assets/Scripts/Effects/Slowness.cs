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
        if (currentTime < effectParameters.duration)
        {
            currentTime += Time.deltaTime;

            entity.speedCurrent = SetEffectPower(entity.speed, true);
        }
        else
        {
            EndEffect();

            Destroy(gameObject);
        }
    }

    public override void EndEffect()
    {
        currentTime = effectParameters.duration;
        entity.speedCurrent = entity.speed;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

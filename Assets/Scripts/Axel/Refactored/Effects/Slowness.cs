using System.Collections;
using UnityEngine;

public class Slowness : Effect
{
    public void Start()
    {
        SetEffect(entity.currentSpeed);
    }

    private void Update()
    {
        if (currentTime < effectDuration)
        {
            currentTime += Time.deltaTime;

            entity.currentSpeed = SetEffectPower(entity.speed);
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
        entity.currentSpeed = entity.speed;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

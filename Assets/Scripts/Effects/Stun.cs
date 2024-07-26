using System.Collections;
using UnityEngine;

public class Stun : Effect
{
    public void Start()
    {
        SetEffect();

        effectParameters.duration = SetEffectPower(effectParameters.duration, false);
    }

    private void Update()
    {
        if (currentTime < effectParameters.duration)
        {
            currentTime += Time.deltaTime;

            entity.canMove = false;
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
        entity.canMove = true;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
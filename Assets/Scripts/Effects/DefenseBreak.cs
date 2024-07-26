using System.Collections;
using UnityEngine;

public class DefenseBreak : Effect
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

            entity.defenseCurrent = 0;
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
        entity.defenseCurrent = entity.defense;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
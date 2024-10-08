using System.Collections;
using UnityEngine;

public class DefenseBreak : Effect
{
    public void Start()
    {
        SetEffect();

        parameters.durationCurrent = SetEffectPower(parameters.duration, true) + 1;
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
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
        currentTime = parameters.duration;
        entity.defenseCurrent = entity.defense;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
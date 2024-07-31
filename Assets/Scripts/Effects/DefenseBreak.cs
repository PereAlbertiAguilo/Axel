using System.Collections;
using UnityEngine;

public class DefenseBreak : Effect
{
    float currentDuration;

    public void Start()
    {
        SetEffect();

        currentDuration = SetEffectPower(parameters.duration, false);
    }

    private void Update()
    {
        if (currentTime < currentDuration)
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
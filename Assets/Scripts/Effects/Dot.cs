using System.Collections;
using UnityEngine;

public class Dot : Effect
{
    float currentDotTime = 0;

    public void Start()
    {
        SetEffect();
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            if(currentDotTime < .4f)
            {
                currentDotTime += Time.deltaTime;
            }
            else
            {
                currentDotTime = 0;

                targetEntity.RemoveHealth(SetEffectPower(attackerEntity.damageCurrent * 0.60f, false));
            }
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
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}

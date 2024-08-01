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

                entity.RemoveHealth(SetEffectPower(5f * entity.health / 100, false));
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

using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Stun : Effect
{
    Animator animator;
    SpriteAnimation spriteAnimation;

    public void Start()
    {
        SetEffect();

        parameters.durationCurrent = SetEffectPower(parameters.duration, false);

        if(targetEntity.TryGetComponent(out animator)) animator.enabled = false;
        if(targetEntity.TryGetComponent(out spriteAnimation)) spriteAnimation.enabled = false;
    }

    private void Update()
    {
        if (currentTime < parameters.durationCurrent)
        {
            currentTime += Time.deltaTime;

            targetEntity.canMove = false;
            targetEntity.canDealDamage = false;
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
        targetEntity.canMove = true;
        targetEntity.canDealDamage = true;

        if (animator != null) animator.enabled = true;
        if (spriteAnimation != null) spriteAnimation.enabled = true;
    }

    private void OnDestroy()
    {
        EndEffect();
    }
}
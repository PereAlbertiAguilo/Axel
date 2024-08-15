using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Entity
{
    public bool canBeKnockbacked = true;

    [Space]

    public Rigidbody2D _enemyRigidbody;
    public SpriteRenderer _enemyRenderer;

    [Space]

    [HideInInspector] public EnemiesManager enemiesManager;

    public override void Awake()
    {
        base.Awake();

        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _enemyRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();

        if(transform.parent.TryGetComponent(out enemiesManager)) enemiesManager.AddToEnemiesList(gameObject);
    }

    public void OnHit()
    {
        if (healthCurrent <= 0) return;

        _enemyRenderer.color = PlayerController.instance.damagedColor;

        CancelInvoke();

        Invoke(nameof(ResetDamagedColor), .1f);

        if (PlayerController.instance.effectsManager.appliesEffects)
        {
            effectsManager.ApplyEffect(PlayerController.instance.effectsManager);
        }
    }

    public virtual void MoveReset()
    {
        canMove = true;
    }

    public void ResetDamagedColor()
    {
        _enemyRenderer.color = Color.white;
    }

    public virtual void DeactivateFollowState()
    {
        canMove = false;
    }

    private void OnDisable()
    {
        if (transform.parent.TryGetComponent(out enemiesManager)) enemiesManager.RemoveFromEnemiesList(gameObject);
    }
}

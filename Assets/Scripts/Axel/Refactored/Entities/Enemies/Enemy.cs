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

    public virtual void Start()
    {
        if(transform.parent.TryGetComponent(out enemiesManager)) enemiesManager.AddToEnemiesList(gameObject);
    }

    public void OnHit()
    {
        if (canMove && canBeKnockbacked)
        {
            _enemyRigidbody.velocity = Vector2.zero;
            _enemyRigidbody.AddForce(-Direction.Normalized(PlayerController.instance.transform.position, transform.position) * 1000, ForceMode2D.Impulse);
        }

        _enemyRenderer.color = PlayerController.instance.damagedColor;

        CancelInvoke();

        DeactivateFollowState();

        Invoke(nameof(MoveReset), .1f);

        if (PlayerController.instance.appliesEffects)
        {
            ApplyEffect(PlayerController.instance);
        }
    }

    public virtual void MoveReset()
    {
        canMove = true;
        _enemyRigidbody.velocity = Vector2.zero;
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

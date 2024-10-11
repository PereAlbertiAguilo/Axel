using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Entity
{
    [HideInInspector] public Rigidbody2D _enemyRigidbody;
    [HideInInspector] public EnemiesManager enemiesManager;
    [HideInInspector] public AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();

        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();

        Transform parent = transform.parent;

        if (parent != null) enemiesManager = parent.GetComponent<EnemiesManager>();
    }

    public override void Start()
    {
        base.Start();

        if (enemiesManager != null) enemiesManager.AddToEnemiesList(gameObject);
    }

    public void OnHit()
    {
        if (healthCurrent <= 0) return;

        StartCoroutine(DamagedAnimation(.5f));
        StartCoroutine(JiggleAnimation(.5f));

        if (PlayerController.instance.effectsManager.appliesEffects)
        {
            effectsManager.ApplyEffect(PlayerController.instance.effectsManager);
        }
    }

    public virtual void MoveReset()
    {
        canMove = true;

        if(audioSource != null)
        {
            audioSource.mute = canMove;
        }
    }

    public virtual void DeactivateFollowState()
    {
        canMove = false;

        if (audioSource != null)
        {
            audioSource.mute = canMove;
        }
    }

    public virtual void OnDisable()
    {
        if(enemiesManager != null) enemiesManager.RemoveFromEnemiesList(gameObject);
    }
}

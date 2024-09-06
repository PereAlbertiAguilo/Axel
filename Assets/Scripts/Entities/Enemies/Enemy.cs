using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Entity
{
    [HideInInspector] public Rigidbody2D _enemyRigidbody;
    [HideInInspector] public SpriteRenderer _enemyRenderer;
    [HideInInspector] public EnemiesManager enemiesManager;
    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public Color defaultColor;

    public override void Awake()
    {
        base.Awake();

        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _enemyRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();

        Transform parent = transform.parent;

        if (parent != null) enemiesManager = parent.GetComponent<EnemiesManager>();

        defaultColor = _enemyRenderer.color;
    }

    public override void Start()
    {
        base.Start();

        if (enemiesManager != null) enemiesManager.AddToEnemiesList(gameObject);
    }

    public void OnHit()
    {
        if (healthCurrent <= 0) return;

        StopAllCoroutines();

        _enemyRenderer.color = PlayerController.instance.damagedColor;

        StartCoroutine(ResetDamagedColor(defaultColor));

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
            audioSource.Play();
        }
    }

    public IEnumerator ResetDamagedColor(Color defaultColor)
    {
        yield return new WaitForSeconds(.2f);

        _enemyRenderer.color = defaultColor;
    }

    public virtual void DeactivateFollowState()
    {
        canMove = false;

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public virtual void OnDisable()
    {
        if(enemiesManager != null) enemiesManager.RemoveFromEnemiesList(gameObject);
    }
}

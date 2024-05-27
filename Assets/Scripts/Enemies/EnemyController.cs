using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
using System.IO;

public class EnemyController : MonoBehaviour
{
    bool canMove = true;

    Vector3 heading;
    Vector3 direction;

    Transform player;
    PlayerController playerController;
    DamageManager _damageManager;

    [HideInInspector] public Rigidbody2D _enemyRigidbody;

    AIPath _aIPath;
    AIDestinationSetter _destinationSetter;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _aIPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _damageManager = GetComponent<DamageManager>();
    }

    private void Start()
    {
        EnemiesManager.instance.AddToEnemiesList(gameObject);

        playerController = FindAnyObjectByType<PlayerController>();
        player = playerController.transform;

        _destinationSetter.target = player;
    }

    private void Update()
    {
        if (playerController._playerHealth.currentHealth <= 0 && canMove) 
        {
            canMove = false;
            DeactivateFollowState();
        }
    }

    public void OnHit()
    {
        CancelInvoke();

        canMove = false;
        Invoke(nameof(MoveReset), .45f);

        DeactivateFollowState();

        heading = player.position - transform.position;
        direction = heading / heading.magnitude;

        _enemyRigidbody.velocity = Vector2.zero;
        _enemyRigidbody.AddForce(-direction * 100, ForceMode2D.Impulse);
    }

    void MoveReset()
    {
        _enemyRigidbody.velocity = Vector2.zero;
        canMove = true;

        DeactivateFollowState();
    }

    void DeactivateFollowState()
    {
        if (_aIPath != null) _aIPath.canMove = canMove;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerController != null && playerController.canTakeDamage)
        {
            playerController.canTakeDamage = false;
            playerController.Invoke(nameof(playerController.DamageReset), .5f);

            heading = player.position - transform.position;
            direction = heading / heading.magnitude;

            playerController.StartCoroutine(playerController.IFrameAnimation(.5f, 1, true,
                Vector3.Distance(player.position, transform.position) > .1f ? direction : Vector2.zero));

            playerController._playerHealth.RemoveHealth(_damageManager.damage, playerController.Death);
        }
    }

    private void OnDisable()
    {
        EnemiesManager.instance.RemoveFromEnemiesList(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public bool canMove = true;
    public bool canBeKnockbacked = true;

    Vector3 heading;
    Vector3 direction;

    Transform player;
    PlayerController playerController;
    DamageManager _damageManager;

    [HideInInspector] public Rigidbody2D _enemyRigidbody;

    [HideInInspector] public AIPath _aIPath;
    AIDestinationSetter _destinationSetter;
    SpriteRenderer _spriteRenderer;
    EnemiesManager enemiesManager;

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
        enemiesManager = transform.parent.GetComponent<EnemiesManager>();
        if (enemiesManager != null) enemiesManager.AddToEnemiesList(gameObject);

        playerController = FindAnyObjectByType<PlayerController>();
        player = playerController.transform;

        _destinationSetter.target = player;
    }

    private void Update()
    {
        if (playerController._playerHealth.currentHealth <= 0) 
        {
            canMove = false;
            DeactivateFollowState();
        }
    }

    public void OnHit()
    {
        if (canMove && canBeKnockbacked)
        {
            CancelInvoke();

            canMove = false;
            Invoke(nameof(MoveReset), .2f);

            DeactivateFollowState();

            heading = player.position - transform.position;
            direction = heading / heading.magnitude;

            _enemyRigidbody.velocity = Vector2.zero;
            _enemyRigidbody.AddForce(-direction * 800, ForceMode2D.Impulse);
        }
    }

    void MoveReset()
    {
        _enemyRigidbody.velocity = Vector2.zero;
        canMove = true;

        DeactivateFollowState();
    }

    public void DeactivateFollowState()
    {
        if (_aIPath != null) _aIPath.canMove = canMove;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerController != null && playerController.canTakeDamage)
        {
            heading = player.position - transform.position;
            direction = heading / heading.magnitude;

            playerController.DamagedIFrames(.75f, true,
                Vector3.Distance(player.position, transform.position) > .1f ? direction : Vector2.zero);
            playerController._playerHealth.RemoveHealth(_damageManager.damage, playerController.Death);
        }
    }

    private void OnDisable()
    {
        enemiesManager.RemoveFromEnemiesList(gameObject);
    }
}

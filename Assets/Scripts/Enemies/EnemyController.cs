using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 20f;

    bool canMove = true;

    Vector3 heading;
    Vector3 direction;

    Transform player;
    PlayerController playerController;
    DamageManager _damageManager;

    [HideInInspector] public Rigidbody2D _enemyRigidbody;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _damageManager = GetComponent<DamageManager>();
    }

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        player = playerController.transform;
    }

    private void Update()
    {
        if (canMove && playerController != null) transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
    }

    public void OnHit()
    {
        _enemyRigidbody.velocity = Vector2.zero;
        heading = player.position - transform.position;
        direction = heading / heading.magnitude;
        _enemyRigidbody.AddForce(-direction * 100, ForceMode2D.Impulse);

        canMove = false;
        Invoke(nameof(MoveReset), .45f);
    }

    void MoveReset()
    {
        _enemyRigidbody.velocity = Vector2.zero;
        canMove = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController != null && playerController.canTakeDamage)
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
}

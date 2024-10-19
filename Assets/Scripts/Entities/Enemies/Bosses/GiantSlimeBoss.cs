using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSlimeBoss : Boss
{
    [SerializeField] GameObject projetile;

    [Space]

    [SerializeField] int spawningEnemies = 4;
    [SerializeField] GameObject spawningEnemy;

    [Space]

    [SerializeField] AudioClip attackSound;

    int attackIndex = 0;
    float currentAttackSpeed;
    bool halfLifeSpawn = false;

    List<GameObject> instances = new List<GameObject>();

    AIPath _aIPath;
    AIDestinationSetter _destinationSetter;

    public override void Awake()
    {
        base.Awake();

        _aIPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();

        currentAttackSpeed = attackSpeedCurrent;
    }

    public override void Start()
    {
        base.Start();

        _destinationSetter.target = PlayerController.instance.transform;

        ObjectPooling.instance.FillPool(instances, projetile, transform, 30);
    }

    public override void Update()
    {
        base.Update();

        _aIPath.canMove = canMove;
        _aIPath.maxSpeed = speedCurrent;

        if (!canMove) return;

        if (currentAttackSpeed <= 0)
        {
            Attack();
        }
        else
        {
            currentAttackSpeed -= Time.deltaTime * timeSpeed;
        }

        if (healthCurrent <= health * 0.5f && !halfLifeSpawn)
        {
            halfLifeSpawn = true;

            SpawnMinions();
        }
    }

    public void Attack()
    {
        StartCoroutine(AnimatorState.ChangeState(_animator, "GiantSlimeAttack"));

        currentAttackSpeed = attackSpeedCurrent;

        switch (attackIndex)
        {
            case 0:
                StartCoroutine(BulletCircle(4));
                currentAttackSpeed = attackSpeedCurrent / 1.5f;
                break;
            case 1:
                StartCoroutine(BulletCircle(8));
                break;
            case 2:
                StartCoroutine(BulletCircle(12));
                currentAttackSpeed = attackSpeedCurrent / 2f;
                break;
            case 3:
                StartCoroutine(BulletCircle(16));
                currentAttackSpeed = attackSpeedCurrent * 2.5f;
                break;
        }

        attackIndex = attackIndex < 3 ? (attackIndex + 1) : 0;
    }

    public override void OnDeath()
    {
        SpawnMinions();

        ObjectPooling.instance.EmptyPool(instances);

        base.OnDeath();
    }

    public IEnumerator BulletCircle(int bulletsAmount)
    {
        float randomAngleOffset = Random.Range(0, 360);
        float angle = 360 / (float)bulletsAmount;

        yield return new WaitForSeconds(.8f);
        if ((int)timeSpeed != 1) yield return new WaitForSeconds(.8f - (.8f * timeSpeed));

        Audio.instance.PlayOneShot(attackSound, .5f, true);

        for (int i = 0; i < bulletsAmount; i++)
        {
            GameObject instance = ObjectPooling.instance.InstatiateObject(instances, projetile, 
                transform.position, Quaternion.Euler(0, 0, angle * i + randomAngleOffset), transform);

            if (instance.TryGetComponent(out EnemyCollider enemyCollider))
            {
                enemyCollider.enemy = this;
            }
        }
    }

    void SpawnMinions()
    {
        float angleOffset = 360 / (float)spawningEnemies;

        float rad = angleOffset * Mathf.Deg2Rad;

        for (int i = 0; i < spawningEnemies; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(rad * i), Mathf.Sin(rad * i));

            GameObject instance = Instantiate(spawningEnemy, transform.parent);
            instance.transform.position = new Vector2(transform.position.x + (dir.x * 2), transform.position.y + (dir.y * 2));
        }
    }

    public override void StartMovement()
    {
        base.StartMovement();

        _aIPath.canMove = canMove;
    }

    public override void StopMovement()
    {
        base.StopMovement();

        _aIPath.canMove = canMove;
    }
}

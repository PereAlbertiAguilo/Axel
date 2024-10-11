using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSlimeBoss : Boss
{
    List<GameObject> instances = new List<GameObject>();

    [SerializeField] GameObject projetile;

    [Space]

    [SerializeField] int spawningEnemies = 4;
    [SerializeField] GameObject spawningEnemy;

    [Space]

    [SerializeField] AudioClip attackSound;

    int attackIndex = 0;

    float currentAttackSpeed;

    bool halfLifeSpawn = false;
    bool attack = true;

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

        ObjectPooling.instance.FillPool(instances, projetile, transform);
    }

    public override void Update()
    {
        base.Update();

        _aIPath.canMove = canMove;
        _aIPath.maxSpeed = speedCurrent;

        if (!canMove) return;

        if (attack)
        {
            attack = false;

            StartCoroutine(Attack());
        }

        //if (currentAttackSpeed <= 0)
        //{
            
        //}
        //else
        //{
        //    currentAttackSpeed -= Time.deltaTime;
        //}

        if (healthCurrent <= health * 0.5f && !halfLifeSpawn)
        {
            halfLifeSpawn = true;

            SpawnMinions();
        }
    }

    public IEnumerator Attack()
    {
        _animator.Play("GiantSlimeAttack");

        currentAttackSpeed = attackSpeedCurrent;

        switch (attackIndex)
        {
            case 0: StartCoroutine(BulletAttack(4)); break;
            case 1:
                StartCoroutine(BulletAttack(8));
                currentAttackSpeed = attackSpeedCurrent / 1.5f;
                break;
            case 2:
                StartCoroutine(BulletAttack(12));
                currentAttackSpeed = attackSpeedCurrent * 2.5f;
                break;
        }

        attackIndex = attackIndex < 2 ? (attackIndex + 1) : 0;

        yield return new WaitForSeconds(currentAttackSpeed);

        attack = true;
    }

    public override void OnDeath()
    {
        SpawnMinions();

        base.OnDeath();
    }

    public IEnumerator BulletAttack(int bulletsAmount)
    {
        float angleOffset = Random.Range(0, 360);

        float angle = 360 / (float)bulletsAmount;

        yield return new WaitForSeconds(.8f);

        Audio.instance.PlayOneShot(attackSound, .5f, true);

        for (int i = 0; i < bulletsAmount; i++)
        {
            GameObject instance = ObjectPooling.instance.InstatiateObject(instances, projetile, transform.position, Quaternion.Euler(0, 0, angle * i + angleOffset), transform);

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

    public override void MoveReset()
    {
        base.MoveReset();

        _aIPath.canMove = canMove;
    }

    public override void DeactivateFollowState()
    {
        base.DeactivateFollowState();

        _aIPath.canMove = canMove;
    }
}

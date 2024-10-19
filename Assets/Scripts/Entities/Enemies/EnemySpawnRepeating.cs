using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnRepeating : SpawnRepeating
{
    public Enemy enemy;

    public override void Start()
    {
        base.Start();

        foreach (GameObject instance in instances)
        {
            if(instance.TryGetComponent(out EnemyCollider enemyCollider))
            {
                enemyCollider.enemy = enemy;
            }
        }
    }

    public override void Update()
    {
        if (!enemy.canDealDamage || !enemy.canMove) return;

        fireRate = enemy.attackSpeedCurrent;

        base.Update();
    }
}

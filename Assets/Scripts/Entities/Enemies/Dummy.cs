using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    public override void Awake()
    {
        base.Awake();

        enemiesManager = null;
    }

    public override void Start()
    {
        enemiesManager = null;

        base.Start();
    }

    public override void OnDisable()
    {
        enemiesManager = null;

        base.OnDisable();
    }
}

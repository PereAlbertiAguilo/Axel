using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitsManager : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!EnemiesManager.instance.enemiesAlive)
        {
            _animator.SetTrigger("LowerBarriers");

            foreach (Transform t in transform)
            {
                t.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
}

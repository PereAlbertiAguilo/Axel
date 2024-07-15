using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mele : Weapon
{
    PolygonCollider2D weaponCollider;

    public int colliderFrame = 0;

    private void Awake()
    {
        weaponCollider = GetComponent<PolygonCollider2D>();
    }

    public override void Attack()
    {
        StartCoroutine(ActivateCollider());

        base.Attack();
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(keyframeDuration * colliderFrame - keyframeDuration);

        weaponCollider.enabled = true;

        yield return new WaitForSeconds(keyframeDuration);

        weaponCollider.enabled = false;

        yield return new WaitUntil(() => attackState == true);
    }
}

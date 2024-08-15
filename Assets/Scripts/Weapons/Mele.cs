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
        base.Attack();

        StartCoroutine(ActivateCollider());
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(keyframeDuration * colliderFrame / AnimationSpeed());

        Audio.instance.PlayOneShot(Audio.Sound.attack, .35f);

        weaponCollider.enabled = true;

        yield return new WaitForSeconds(keyframeDuration / AnimationSpeed());

        weaponCollider.enabled = false;

        yield return new WaitUntil(() => attackState == true);
    }
}

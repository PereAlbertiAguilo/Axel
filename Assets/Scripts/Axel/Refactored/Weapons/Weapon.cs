using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Element
    {
        water, air, earth, fire, light, dark
    };

    public enum WeaponType
    {
        sword, maces, axes, hammers, dagger, orb,
    };

    [HideInInspector] public Element weaponElement;
    [HideInInspector] public WeaponType weaponType;
    [HideInInspector] public float animationDuration;
    [HideInInspector] public SpriteRenderer weaponRenderer;
    [HideInInspector] public Sprite[] attackSpriteSheet;
    [HideInInspector] public string weaponName;
    [HideInInspector] public Sprite weaponSprite;

    [HideInInspector] public bool attackState = false;

    float horizontalInput;
    float verticalInput = -1;
    float verticalView = -1;

    protected float keyframeDuration = 2;
    int animationIndex = -1;

    Vector3 pos = Vector2.down;

    private void Start()
    {
        keyframeDuration = attackSpriteSheet.Length / 12f / attackSpriteSheet.Length;
    }

    private void Update()
    {
        horizontalInput = UserInput.instance.attackDirInput.x;
        verticalInput = UserInput.instance.attackDirInput.y;

        horizontalInput = Mathf.Abs(horizontalInput) <= .3f ? 0 : horizontalInput;
        verticalInput = Mathf.Abs(verticalInput) <= .3f ? 0 : verticalInput;

        int playerSortingOrder = PlayerController.instance._playerSpriteRenderer.sortingOrder;

        if (!attackState && (horizontalInput != 0 || verticalInput != 0))
        {
            Attack();

            if (verticalInput != 0)
            {
                verticalView = verticalInput;
            }
            else if (horizontalInput != 0)
            {
                verticalView = 1;
            }
        }

        weaponRenderer.sortingOrder = verticalView < 0 ? playerSortingOrder + 1 : playerSortingOrder - 1;
    }

    public virtual void Attack()
    {
        attackState = true;

        StartCoroutine(AttackAnimation());

        pos = transform.position + new Vector3(horizontalInput, verticalInput);

        Invoke(nameof(AttackStateReset), PlayerController.instance.attackSpeedCurrent);

        transform.rotation = Direction.Rotation(pos, transform.position);
    }

    public IEnumerator AttackAnimation()
    {
        Sprite idleSprite = weaponRenderer.sprite;

        float currentTime = 0;

        foreach (Sprite frame in attackSpriteSheet)
        {
            weaponRenderer.sprite = frame;

            currentTime += keyframeDuration;
            animationIndex++;

            yield return new WaitForSeconds(keyframeDuration / AnimationSpeed());
        }

        weaponRenderer.sprite = idleSprite;
    }

    public float AnimationSpeed()
    {
        return animationDuration / PlayerController.instance.attackSpeedCurrent;
    }

    public void AttackStateReset()
    {
        attackState = false;

        CancelInvoke();
    }
}

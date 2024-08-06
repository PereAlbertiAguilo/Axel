using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class Weapon : MonoBehaviour
{
    [HideInInspector] public WeaponManager.Element weaponElement;
    [HideInInspector] public WeaponManager.Type weaponType;
    [HideInInspector] public SpriteRenderer weaponRenderer;
    [HideInInspector] public Sprite[] attackSpriteSheet;
    [HideInInspector] public string weaponName;
    [HideInInspector] public Sprite weaponSprite;
    [HideInInspector] public bool attackState = false;
    [HideInInspector] public int animationFrameRate;
    [HideInInspector] public float animationDuration;

    public float weaponAddedDamage = 0;

    float horizontalInput;
    float verticalInput = -1;
    float verticalView = -1;

    protected float keyframeDuration = 2;
    int animationIndex = -1;

    Vector3 pos = Vector2.down;

    private void Start()
    {
        gameObject.name = "Weapon";

        PlayerController.instance.currentWeapon = this;

        keyframeDuration = attackSpriteSheet.Length / (float)animationFrameRate / attackSpriteSheet.Length;
        animationDuration = keyframeDuration * (attackSpriteSheet.Length + 1);
    }

    private void Update()
    {
        horizontalInput = UserInput.instance.attackDirInput.x;
        verticalInput = UserInput.instance.attackDirInput.y;

        horizontalInput = Mathf.Abs(horizontalInput) <= .3f ? 0 : horizontalInput;
        verticalInput = Mathf.Abs(verticalInput) <= .3f ? 0 : verticalInput;

        int playerSortingOrder = PlayerController.instance._playerSpriteRenderer.sortingOrder;

        if (!attackState && (horizontalInput != 0 || verticalInput != 0) && PlayerController.instance.canMove && PlayerController.instance.canDealDamage)
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
        return PlayerController.instance.attackSpeedCurrent < animationDuration ? (animationDuration / PlayerController.instance.attackSpeedCurrent) : 1;
    }

    public void AttackStateReset()
    {
        attackState = false;

        CancelInvoke();
    }
}

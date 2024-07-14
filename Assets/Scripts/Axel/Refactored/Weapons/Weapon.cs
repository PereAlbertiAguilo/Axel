using System;
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
    [HideInInspector] public Animator weaponAnimator;
    [HideInInspector] public AnimationClip attackAnimation;
    [HideInInspector] public SpriteRenderer weaponRenderer;
    [HideInInspector] public Sprite[] attackSpriteSheet;
    [HideInInspector] public string weaponName;
    [HideInInspector] public Sprite weaponSprite;

    [HideInInspector] public bool attackState = false;

    public Action UseWeapon;

    float horizontalInput;
    float verticalInput = -1;

    protected float keyframeDuration = 0;
    int animationIndex = 0;

    Vector3 pos = Vector2.down;

    private void Start()
    {
        SetAttackAnimation();

        Attack();
    }

    private void Update()
    {
        horizontalInput = UserInput.instance.attackDirInput.x;
        verticalInput = UserInput.instance.attackDirInput.y;

        horizontalInput = Mathf.Abs(horizontalInput) <= .3f ? 0 : horizontalInput;
        verticalInput = Mathf.Abs(verticalInput) <= .3f ? 0 : verticalInput;

        if (!attackState && (horizontalInput != 0 || verticalInput != 0))
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        attackState = true;

        weaponAnimator.SetBool("Attack", true);

        UseWeapon();

        pos = transform.position + new Vector3(horizontalInput, verticalInput);

        Invoke(nameof(AttackStateReset), PlayerController.instance.currentAttackSpeed);

        transform.rotation = Direction.Rotation(pos, transform.position);
    }

    public void SetAttackAnimation()
    {
        attackAnimation.ClearCurves();

        float currentTime = 0;
        keyframeDuration = attackSpriteSheet.Length / attackAnimation.frameRate / attackSpriteSheet.Length;

        for (int i = 0; i < attackSpriteSheet.Length; i++)
        {
            SetFrame(i, currentTime);

            currentTime += keyframeDuration;
            animationIndex++;
        }

        SetFrame(0, currentTime);

        SetAnimationSpeed();

        List<Keyframe> frame = new List<Keyframe>();
        Keyframe key = new Keyframe(currentTime - keyframeDuration, 1);
        frame.Add(key);
        AnimationCurve curve = new AnimationCurve(frame.ToArray());

        attackAnimation.SetCurve("", typeof(Transform), "m_LocalScale.z", curve);
    }

    void SetFrame(int frame, float currentTime)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = nameof(AnimationEvent);
        animationEvent.intParameter = frame;
        animationEvent.time = currentTime;

        attackAnimation.AddEvent(animationEvent);
    }

    void AnimationEvent(int frame)
    {
        weaponRenderer.sprite = attackSpriteSheet[frame];
    }

    public void SetAnimationSpeed()
    {
        weaponAnimator.speed = animationDuration / PlayerController.instance.attackSpeed;
    }

    public void SetWeapon(Action use)
    {
        UseWeapon = use;
    }

    public void AttackStateReset()
    {
        attackState = false;
        weaponAnimator.SetBool("Attack", false);
    }
}

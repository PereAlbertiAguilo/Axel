using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    public Image healthBar;

    public Animator _animator;

    public AudioSource _audioSource;

    public override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public override void Start()
    {
        base.Start();

        Audio.instance.StartCoroutine(Audio.instance.FadeMusicIn(_audioSource, 2.5f, .15f));
        Audio.instance.StartCoroutine(Audio.instance.FadeMusicOut(Audio.instance.musicAudioSource, 2.5f));
    }

    public override void Update()
    {
        base.Update();

        healthBar.fillAmount = healthCurrent / health;
    }

    public override void OnDeath()
    {
        base.OnDeath();

        _audioSource.transform.SetParent(null);
        _audioSource.AddComponent<DestroyOverTime>().lifeTime = 2.6f;

        Audio.instance.StartCoroutine(Audio.instance.FadeMusicOut(_audioSource, 2.5f));
        Audio.instance.StartCoroutine(Audio.instance.FadeMusicIn(Audio.instance.musicAudioSource, 2.5f, .15f));
    }
}

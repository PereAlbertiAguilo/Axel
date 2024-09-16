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

        Audio.instance.StartCoroutine(Audio.instance.FadeAudioIn(_audioSource, 2.5f, .15f));
        Audio.instance.StartCoroutine(Audio.instance.FadeAudioOut(Audio.instance.musicAudioSource, 2.5f));
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
        _audioSource.gameObject.AddComponent<DestroyOverTime>().lifeTime = 5.1f;

        Audio.instance.StartCoroutine(Audio.instance.FadeAudioOut(_audioSource, 5));
        Audio.instance.StartCoroutine(Audio.instance.FadeAudioIn(Audio.instance.musicAudioSource, 5, .15f));
    }
}

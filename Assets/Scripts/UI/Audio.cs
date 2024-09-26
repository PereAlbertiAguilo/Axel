using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    public enum Sound
    {
        click, hurt, interact, slash, swoosh , heal, attack, changeRoom, squash
    }

    public static Audio instance;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(FadeAudioIn(musicAudioSource, 5, .15f));
    }

    // Plays a sound

    public void PlayOneShot(AudioClip ac)
    {
        PlayOneShot(sfxAudioSource, ac, 1);
    }

    public void PlayOneShot(AudioClip ac, float volumeScale, bool randomPich)
    {
        PlayOneShot(CreateAudioSource(ac, randomPich), ac, volumeScale);
    }

    public void PlayOneShot(AudioSource audioSource, AudioClip ac, float volumeScale)
    {
        if (Time.timeScale > 0) audioSource.PlayOneShot(ac, volumeScale);
    }

    public void PlayOneShot(AudioSource audioSource, AudioClip ac, float volumeScale, bool randomPich)
    {
        if (randomPich) audioSource.pitch = UnityEngine.Random.Range(1, 1.5f);

        PlayOneShot(audioSource, ac, volumeScale);
    }

    public void PlayOneShot(Sound sound)
    {
        AudioClip ac = Resources.Load($"Audios/{sound}") as AudioClip;

        PlayOneShot(sfxAudioSource, ac, 1);
    }

    public void PlayOneShot(Sound sound, float volumeScale)
    {
        AudioClip ac = Resources.Load($"Audios/{sound}") as AudioClip;

        PlayOneShot(CreateAudioSource(ac, true), ac, volumeScale);
    }

    public void PlayOneShot(Sound sound, float volumeScale, bool randomPich)
    {
        AudioClip ac = Resources.Load($"Audios/{sound}") as AudioClip;

        PlayOneShot(CreateAudioSource(ac, randomPich), ac, volumeScale);
    }

    AudioSource CreateAudioSource(AudioClip ac, bool randomPich)
    {
        Type[] types = new Type[] { typeof(AudioSource), typeof(DestroyOverTime) };

        GameObject audioSourceObject = new GameObject("AudioSource", types);

        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
        DestroyOverTime destroyOverTime = audioSourceObject.GetComponent<DestroyOverTime>();

        audioSource.outputAudioMixerGroup = sfxMixer;

        destroyOverTime.lifeTime = ac.length;

        if (randomPich) audioSource.pitch = UnityEngine.Random.Range(1, 1.5f);

        return audioSource;
    }

    public void ClickSound()
    {
        PlayOneShot(Sound.click, .4f);
    }

    public IEnumerator FadeAudioIn(AudioSource ac, float delay, float maxVolume)
    {
        float currentTime = 0;

        ac.volume = 0;

        while (currentTime < delay)
        {
            currentTime += Time.unscaledDeltaTime;
            ac.volume = Mathf.Lerp(0, maxVolume, currentTime / delay);

            yield return null;
        }

        ac.volume = maxVolume;
    }

    public IEnumerator FadeAudioOut(AudioSource ac, float delay)
    {
        float currentTime = 0;

        float maxVolume = ac.volume;

        while (currentTime < delay)
        {
            currentTime += Time.unscaledDeltaTime;
            ac.volume = Mathf.Lerp(maxVolume, 0, currentTime / delay);

            yield return null;
        }

        ac.volume = 0;
    }

    public void UpdateSFX(bool activate)
    {
        foreach (AudioSource ac in FindObjectsOfType<AudioSource>(true))
        {
            if (ac.outputAudioMixerGroup == sfxMixer) ac.mute = activate;
        }
    }
}

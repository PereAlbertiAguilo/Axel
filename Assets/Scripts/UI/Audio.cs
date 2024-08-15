using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    public enum Sound
    {
        click, hurt, interact, slash, swoosh , heal, attack
    }

    public static Audio instance;

    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerGroup sfxMixer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Plays a sound
    public void PlayOneShot(AudioSource audioSource, AudioClip ac, float volumeScale)
    {
        audioSource.PlayOneShot(ac, volumeScale);
    }

    public void PlayOneShot(AudioClip ac)
    {
        PlayOneShot(sfxAudioSource, ac, 1);
    }

    public void PlayOneShot(AudioClip ac, float volumeScale, bool randomPich)
    {
        PlayOneShot(sfxAudioSource, ac, volumeScale);
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
}

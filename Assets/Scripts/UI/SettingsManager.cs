using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] VolumeProfile volumeProfile;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Toggle fullScreenToggle;

    LiftGammaGain brightness;

    private void Start()
    {
        LiftGammaGain tmp;
        if (volumeProfile.TryGet(out tmp)) brightness = tmp;

        float musicValue = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 2;
        float sfxValue = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 2;
        float brightnessValue = PlayerPrefs.HasKey("brightnessValue") ? PlayerPrefs.GetFloat("brightnessValue") : 0;
        bool fullscreenValue = PlayerPrefs.HasKey("fullScreenActive") ? (PlayerPrefs.GetInt("fullScreenActive") == 0 ? false : true) : true;

        musicSlider.value = musicValue;
        Music(musicValue);
        sfxSlider.value = sfxValue;
        SFX(sfxValue);
        brightnessSlider.value = brightnessValue;
        Brightness(brightnessValue);
        fullScreenToggle.isOn = fullscreenValue;
        FullScreen(fullscreenValue);
    }

    public void Music(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);

        volume = Mathf.Clamp(volume, 0.0001f, musicSlider.value / musicSlider.maxValue);

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SFX(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);

        volume = Mathf.Clamp(volume, 0.0001f, sfxSlider.value / sfxSlider.maxValue);

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void Brightness(float value)
    {
        LiftGammaGain tmp;
        if (volumeProfile.TryGet(out tmp)) brightness = tmp;

        PlayerPrefs.SetFloat("brightnessValue", value);

        value = Mathf.Clamp(value, -brightnessSlider.value / (brightnessSlider.minValue * 2), brightnessSlider.value / (brightnessSlider.maxValue / 2));

        brightness.gamma.value = new Vector4(1f, 1f, 1f, value);
    }

    public void FullScreen(bool active)
    {
        PlayerPrefs.SetInt("fullScreenActive", active ? 1 : 0);

        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, active);
    }
}

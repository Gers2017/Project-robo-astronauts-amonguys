using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public static string volume_key = "volume";
    public static string mouse_key = "mouse";

    const float default_volume = -10f;
    const float default_mouse = 0.5f;

    [SerializeField] AudioMixer audio_mixer;
    [SerializeField] Slider volume_slider;
    [SerializeField] Slider mouse_slider;

    void Awake()
    {
        volume_slider.value = PlayerPrefs.GetFloat(volume_key, default_volume);
        mouse_slider.value = PlayerPrefs.GetFloat(mouse_key, default_mouse);
    }

    public void OnVolumeChange(float value)
    {
        audio_mixer.SetFloat("volume", value);
        PlayerPrefs.SetFloat(volume_key, value);
    }

    public void OnMouseSensitivityChange(float value)
    {
        PlayerPrefs.SetFloat(mouse_key, value);
    }
}

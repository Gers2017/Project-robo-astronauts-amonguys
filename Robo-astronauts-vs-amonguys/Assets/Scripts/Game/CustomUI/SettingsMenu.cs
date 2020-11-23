using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace CustomUI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] AudioMixer audio_mixer;
        [SerializeField] Slider volume_slider;
        [SerializeField] Slider mouse_slider;

        void Awake()
        {
            float volume = GameSettings.GetMasterVolume();
            SetAudioMixerVolume(volume);
            volume_slider.value = volume;
            mouse_slider.value = GameSettings.GetMouseSensitivity();
            GetComponent<RectTransform>().position = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void OnVolumeChange(float value)
        {
            SetAudioMixerVolume(value);
            GameSettings.SetMasterVolume(value);
        }

        private void SetAudioMixerVolume(float value)
        {
            audio_mixer.SetFloat("volume", value);
        }

        public void OnMouseSensitivityChange(float value)
        {
            GameSettings.SetMouseSensitivity(value);
        }
    }
}

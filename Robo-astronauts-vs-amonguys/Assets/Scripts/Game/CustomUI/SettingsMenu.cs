using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;

namespace CustomUI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] Slider volume_slider;
        [SerializeField] Slider mouse_slider;
        [SerializeField] Slider postProcessing_slider;

        void OnEnable()
        {
            volume_slider.value = GameSettings.GetMasterVolume();
            mouse_slider.value = GameSettings.GetMouseSensitivity();
            postProcessing_slider.value = GameSettings.GetPostProcessingWeight();
        }

        public void OnVolumeChange(float value)
        {
            GameSettings.SetMasterVolume(value);
        }

        public void OnMouseSensitivityChange(float value)
        {
            GameSettings.SetMouseSensitivity(value);
        }

        public void OnPPVolumeWightChange(float value)
        {
            GameSettings.SetPostProcessingWeight(value);
        }

        public void OnQualityChange(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

    }
}

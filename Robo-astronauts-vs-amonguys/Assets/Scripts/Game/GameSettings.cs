using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    
    public static class GameSettings
    {
        public static event Action<float> OnMasterVolumeChange;
        public static event Action<float> OnPostProcessingWeightChange;

        public static string volumeKey = "volume";
        public static string mouseKey = "mouse";
        public static string postProcessingWeightKey = "postProcessingVolumeWeight";
        
        static float defaultVolume = -10f;
        static float minVolume = -80f;
        static float defaultMouseSensitivity = 0.5f;
        static float minMouseSensitivity = 0.1f;
        static float defaultPostProcessingWeight = 1f;

        //Master volume
        public static void SetMasterVolume(float value)
        {
            value = Mathf.Max(minVolume, value);
            PlayerPrefs.SetFloat(volumeKey, value);
            OnMasterVolumeChange?.Invoke(value);
        }

        public static float GetMasterVolume()
        {
            return PlayerPrefs.GetFloat(volumeKey, defaultVolume);
        }

        //Mouse Sensitivity
        public static void SetMouseSensitivity(float value)
        {
            value = Mathf.Max(minMouseSensitivity, value);
            PlayerPrefs.SetFloat(mouseKey, value);
        }

        public static float GetMouseSensitivity()
        {
            return PlayerPrefs.GetFloat(mouseKey, defaultMouseSensitivity);
        }

        public static void SetPostProcessingWeight(float value)
        {
            value = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(postProcessingWeightKey, value);
            OnPostProcessingWeightChange?.Invoke(value);
        }

        public static float GetPostProcessingWeight()
        {
            return PlayerPrefs.GetFloat(postProcessingWeightKey, defaultPostProcessingWeight);
        }

    }
}

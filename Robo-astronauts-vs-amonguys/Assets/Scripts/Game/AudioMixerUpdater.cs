using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace GameManagement
{
    public class AudioMixerUpdater : MonoBehaviour
    {
        [SerializeField] AudioMixer audio_mixer;

        private void Awake()
        {
            SetAudioMixerVolume(GameSettings.GetMasterVolume());
            GameSettings.OnMasterVolumeChange += SetAudioMixerVolume;    
        }

        private void SetAudioMixerVolume(float value)
        {
            audio_mixer.SetFloat("volume", value);
        }
    }   
}

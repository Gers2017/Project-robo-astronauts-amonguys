using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace GameManagement
{
    public class PostProcessingManager : MonoBehaviour
    {
        [SerializeField] private PostProcessVolume volume;
        
        private void Awake()
        {
            //Set the last saved weight preference
            SetVolumeWight(GameSettings.GetPostProcessingWeight());

            GameSettings.OnPostProcessingWeightChange += SetVolumeWight;
        }

        void SetVolumeWight(float value)
        {
            volume.weight = value;
        }
    }
}
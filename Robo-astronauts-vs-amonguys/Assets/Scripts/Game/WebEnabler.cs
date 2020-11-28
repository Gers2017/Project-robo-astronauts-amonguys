using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class WebEnabler : MonoBehaviour
    {
        [SerializeField] bool enableOnWeb = false;
        private void Awake()
        {
            if(Application.platform.Equals(RuntimePlatform.WebGLPlayer))
            {
                gameObject.SetActive(enableOnWeb);
            }
        }
    }
}
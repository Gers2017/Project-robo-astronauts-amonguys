using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Secrets
{
    public class RobiSkinManager : MonoBehaviour
    {
        private static RobiSkinManager _instance;
        public static RobiSkinManager instance {get => _instance; private set => _instance = value;}

        //Holds the active skin data
        [SerializeField] SkinData activeSkin;
        public SkinData ActiveSkin => activeSkin;
        public event Action<SkinData> OnActiveSkinChange;
        
        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetActiveSkin(SkinData skin)
        {
            activeSkin = skin;
            OnActiveSkinChange?.Invoke(activeSkin);
        }
    }
}
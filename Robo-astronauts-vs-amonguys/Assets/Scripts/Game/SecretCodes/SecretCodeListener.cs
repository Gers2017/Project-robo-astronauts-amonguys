using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Secrets
{
    public class SecretCodeListener : MonoBehaviour
    {
        const string SecretCode = "gold";
        private bool isSecretCodeActive = false;
        [SerializeField] TMP_InputField secretCodeField;
        [SerializeField] GameObject secretCodePanel;
        [SerializeField] SkinData normalSkin;
        [SerializeField] SkinData secretSkin;

        void Start()
        {
            secretCodeField.gameObject.SetActive(false);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                //Start as false, the code turns true
                isSecretCodeActive = !isSecretCodeActive;
                //So the input is active
                ActiveSecretField(isSecretCodeActive);
            }
        }

        public void ActiveSecretField(bool active)
        {
            secretCodeField.gameObject.SetActive(active);
        }

        public void CheckCode(string code)
        {
            if(code == SecretCode){
                secretCodePanel.SetActive(true);
                ActiveSecretField(false);
            }
        }

        public void SetNormalSkin()
        {
            RobiSkinManager.instance.SetActiveSkin(normalSkin);
        }

        public void SetSecretSkin()
        {
            RobiSkinManager.instance.SetActiveSkin(secretSkin);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace CustomUI
{
    public class MainMenu : MonoBehaviour
    {
        const string Play_scene_name = "PlayScene";
        public GameObject exit_button;

        private void Start() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        public void GotoPlayScene()
        {
            if(SceneManager.GetSceneByName(Play_scene_name) != null)
            {
                SceneManager.LoadScene(Play_scene_name);
            }
        }

        public void GotoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
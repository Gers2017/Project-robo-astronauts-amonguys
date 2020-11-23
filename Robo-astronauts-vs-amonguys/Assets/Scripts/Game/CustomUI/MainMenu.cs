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
        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        public void GotoPlayScene()
        {
            if(SceneManager.GetSceneByName(Play_scene_name) != null)
            {
                SceneManager.LoadScene(Play_scene_name);
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
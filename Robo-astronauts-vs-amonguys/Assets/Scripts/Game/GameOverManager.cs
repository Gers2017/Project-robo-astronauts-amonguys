using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{

    public class GameOverManager : MonoBehaviour
    {
        public float timeReloadScene = 2f;

        void Start()
        {
            PlayerController.OnPlayerDie += HandlePlayerDies;
        }

        private void HandlePlayerDies(PlayerController player)
        {
            PlayerController.OnPlayerDie -= HandlePlayerDies;
            ReloadCurrentScene();
        }

        public void LoadFirstScene()
        {
            StartCoroutine(ReloadSceneCoroutine(0));      
        }
        public void ReloadCurrentScene()
        {
            StartCoroutine(ReloadSceneCoroutine(SceneManager.GetActiveScene().buildIndex));      
        }

        IEnumerator ReloadSceneCoroutine(int index)
        {
            yield return new WaitForSeconds(timeReloadScene);
            SceneManager.LoadScene(index);
        }
    }
}
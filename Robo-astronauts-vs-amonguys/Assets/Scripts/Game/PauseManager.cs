using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameManagement
{

    public class PauseManager : MonoBehaviour
    {
        public bool isPaused;
        public static event Action<bool> OnGamePause;
        public UnityEvent OnPauseEvent;
        public UnityEvent OnUnPauseEvent;

        private void LateUpdate()
        {
            if(Input.GetButtonDown("Pause"))
            {
                if(isPaused)
                    UnPause();
                else
                    Pause();
            }
        }

        public void Pause()
        {
            isPaused = !isPaused;
            Time.timeScale = 0f;
            OnGamePause?.Invoke(isPaused);
            OnPauseEvent.Invoke();
        }

        public void UnPause()
        {
            isPaused = !isPaused;
            Time.timeScale = 1f;
            OnGamePause?.Invoke(isPaused);
            OnUnPauseEvent.Invoke();
        }

    }
}

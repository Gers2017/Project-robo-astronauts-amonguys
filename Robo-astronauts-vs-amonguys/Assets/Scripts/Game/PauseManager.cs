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
        public static Action<bool> OnGamePause;
        public UnityEvent OnPauseEvent;
        public UnityEvent OnUnPauseEvent;

        private void LateUpdate()
        {
            if(Input.GetButtonDown("Pause"))
            {
                Pause();
            }
        }

        public void Pause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            OnGamePause?.Invoke(isPaused);
            if(isPaused)
                OnPauseEvent.Invoke();
            else
                OnUnPauseEvent.Invoke();
        }

    }
}

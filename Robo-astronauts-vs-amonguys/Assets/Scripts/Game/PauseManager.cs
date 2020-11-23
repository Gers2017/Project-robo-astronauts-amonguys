using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    [SerializeField] GameObject pauseMenu;
    public static Action<bool> OnPause;

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
        pauseMenu.SetActive(isPaused);
        OnPause?.Invoke(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

}

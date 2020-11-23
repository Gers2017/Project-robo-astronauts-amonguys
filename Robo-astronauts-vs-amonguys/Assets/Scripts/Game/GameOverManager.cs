using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public float timeReloadScene = 2f;
    private CameraController cameraController;
    void Start()
    {
        PlayerController.OnPlayerDie += HandlePlayerDies;
        cameraController = FindObjectOfType<CameraController>();
    }

    private void HandlePlayerDies(PlayerController player)
    {
        PlayerController.OnPlayerDie -= HandlePlayerDies;
        cameraController.enabled = false;
        StartCoroutine(ReloadScene());        
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(timeReloadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }

}

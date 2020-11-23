using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        StartCoroutine(ReloadScene());        
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(timeReloadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }

}

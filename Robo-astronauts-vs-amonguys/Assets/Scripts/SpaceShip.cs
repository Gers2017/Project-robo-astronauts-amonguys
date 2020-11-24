using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
    public bool isShipEnabled;
    public int sceneIndex;
    public UnityEvent OnAreaEnter;

    public void EnableShip()
    {
        isShipEnabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isShipEnabled && other.CompareTag("Player"))
        {
            OnAreaEnter.Invoke();
            //Load last scene
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
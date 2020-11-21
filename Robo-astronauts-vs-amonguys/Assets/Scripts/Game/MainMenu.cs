using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    const string Play_scene_name = "PlayScene";
    
    private void Awake()
    {
        Cursor.visible = true;
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

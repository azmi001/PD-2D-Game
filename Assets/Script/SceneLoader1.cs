using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader1 : MonoBehaviour
{
    public string scenename;

    public void LoadScene()
    {
        SceneManager.LoadScene(scenename);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

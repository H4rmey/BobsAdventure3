using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BetterButton : MonoBehaviour
{
    public string[] scenes;

    public void GotoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GotoRandomScene()
    {
        SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length)]);
    }
}

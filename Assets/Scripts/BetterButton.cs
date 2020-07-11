using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BetterButton : MonoBehaviour
{
    private Button button;
    public string scene;

    void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(buttonPressed);
    }

    void buttonPressed()
    {
        SceneManager.LoadScene(scene);
    }
}

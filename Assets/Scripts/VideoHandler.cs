using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    public      float       videoDuration;
    public      string      scene;
    private     float       timer;

    public bool howToPlay;
    public string[] scenes;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > videoDuration || Input.anyKeyDown)
            if (!howToPlay)
                SceneManager.LoadScene(scene);
            else
                SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length)]);
    }
}

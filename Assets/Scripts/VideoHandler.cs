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

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > videoDuration)
            SceneManager.LoadScene(scene);
    }
}

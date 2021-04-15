using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraSingleton : MonoBehaviour
{

    public int musicToPlay;
    private bool musicStarted;
    private void Awake()
    {
        int cameraCount = FindObjectsOfType<Camera>().Length;
        if (cameraCount > 1)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}

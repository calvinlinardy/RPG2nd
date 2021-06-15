using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraSingleton : MonoBehaviour
{
    private void Awake()
    {
        int cameraCount = FindObjectsOfType<Camera>().Length;
        if (cameraCount > 1)
        {
            Destroy(gameObject);
        }
    }
}

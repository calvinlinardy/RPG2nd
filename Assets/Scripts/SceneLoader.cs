using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        int sceneLoaderCount = FindObjectsOfType<SceneLoader>().Length;
        if (sceneLoaderCount > 1)
        {
            Destroy(gameObject);
        }
    }
    public void EnteringTeleport(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
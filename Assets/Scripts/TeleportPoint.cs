using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField] string sceneToLoad = null;
    public string spawnPointName = "SpawnPoint";

    SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sceneLoader.EnteringTeleport(sceneToLoad);
        }
        Debug.Log(sceneToLoad);
    }
}
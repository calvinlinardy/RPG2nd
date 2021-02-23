using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField] string sceneToLoad = null;
    [SerializeField] float waitToLoad = 1f;
    private bool loadAfterFade;
    public string spawnPointName = "SpawnPoint";


    SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Update()
    {
        if (loadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                loadAfterFade = false;
                sceneLoader.EnteringTeleport(sceneToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            loadAfterFade = true;
            UIFade.instance.FadeToBlack();
            Player.instance.FreezeCharacter();
        }
        Debug.Log(sceneToLoad);
    }
}
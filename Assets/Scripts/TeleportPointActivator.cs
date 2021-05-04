using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointActivator : MonoBehaviour
{
    public GameObject spawnPoint, otherSpawnPoint;
    public GameObject teleportPoint, otherTeleportPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            spawnPoint.gameObject.SetActive(true);
            teleportPoint.gameObject.SetActive(true);
            otherSpawnPoint.gameObject.SetActive(false);
            otherTeleportPoint.gameObject.SetActive(false);
        }
    }
}

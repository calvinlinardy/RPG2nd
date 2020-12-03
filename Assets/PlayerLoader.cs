using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] GameObject player = null;

    private void Start()
    {
        if (PlayerController.instance == null)
        {
            Instantiate(player);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] GameObject player = null;

    private void Start()
    {
        if (Player.instance == null)
        {
            Instantiate(player);
        }
    }
}

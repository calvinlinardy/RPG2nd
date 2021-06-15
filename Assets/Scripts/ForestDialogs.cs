using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestDialogs : MonoBehaviour
{
    public GameObject dialogs;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Forest")
        {
            dialogs.gameObject.SetActive(false);
        }
        else
        {
            dialogs.gameObject.SetActive(true);
        }
    }
}

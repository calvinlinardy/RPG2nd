using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicBox : MonoBehaviour
{
    public static musicBox instance;
    public int musicToPlay;
    private bool musicStarted;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }

        if (BattleManager.instance.battleActive == true)
        {
            AudioManager.instance.StopMusic(musicToPlay);
            musicStarted = false;
            if (!musicStarted)
            {
                musicStarted = true;
                AudioManager.instance.PlayBGM(3);
            }
        }
    }
}

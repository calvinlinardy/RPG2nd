﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx;
    public AudioSource[] bgm;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        int audioManagerCount = FindObjectsOfType<AudioManager>().Length;
        if (audioManagerCount > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(int soundToPlay)
    {
        sfx[soundToPlay].Play();
    }

    public void PlayBGM(int musicToPlay)
    {
        StopMusic();

        if (musicToPlay < bgm.Length)
        {
            bgm[musicToPlay].Play();
        }
    }

    public void StopMusic()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string newGameScene;
    public GameObject continueButton;
    public GameObject buttons, howToPlayPanel;
    public bool hasToLoadScene = false;
    public static MainMenu instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        /*if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
        hasToLoadScene = true;
        GameManager.instance.loadFromMainMenu = true;
    }

    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        buttons.SetActive(false);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        buttons.SetActive(true);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game.");
    }

    public void PlayButtonSound(int SFx)
    {
        AudioManager.instance.PlaySFX(SFx);
    }
}

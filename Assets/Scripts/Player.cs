﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    float x = 0;
    float movementX = 0;
    float movementY = 0;

    //Cache references
    Rigidbody2D myRb;
    Animator myAnim;
    TeleportPoint teleportPoint;

    public static Player instance;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        x = movementSpeed;

        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += LoadState;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        teleportPoint = FindObjectOfType<TeleportPoint>();

        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        myRb.velocity = new Vector2(movementX, movementY) * x;

        myAnim.SetFloat("moveX", myRb.velocity.x);
        myAnim.SetFloat("moveY", myRb.velocity.y);

        if (movementX == 1 || movementX == -1 || movementY == 1 || movementY == -1)
        {
            myAnim.SetFloat("lastMoveX", movementX);
            myAnim.SetFloat("lastMoveY", movementY);
        }
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        Player.instance.transform.position = GameObject.Find(teleportPoint.spawnPointName).transform.position;
        UIFade.instance.FadeFromBlack();
        x = movementSpeed;
    }

    public void FreezeCharacter()
    {
        x = 0f;
    }
}

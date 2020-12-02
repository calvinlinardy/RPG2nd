﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    float movementX = 0;
    float movementY = 0;

    //Cache references
    Rigidbody2D myRb;
    Animator myAnim;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        myRb.velocity = new Vector2(movementX, movementY) * movementSpeed;

        myAnim.SetFloat("moveX", myRb.velocity.x);
        myAnim.SetFloat("moveY", myRb.velocity.y);

        if (movementX == 1 || movementX == -1 || movementY == 1 || movementY == -1)
        {
            myAnim.SetFloat("lastMoveX", movementX);
            myAnim.SetFloat("lastMoveY", movementY);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ijo : MonoBehaviour
{
    float movementSpeed = 0f;
    public float stoppingDistance = 7f;
    float movementX = 0;
    float movementY = 0;
    public GameObject umbiPlayer;

    private Transform target;

    //Cache references
    Rigidbody2D myRb;
    Animator myAnim;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        target = umbiPlayer.GetComponent<Transform>();
    }

    void Update()
    {
        movementSpeed = Player.instance.movementSpeed;

        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            movementX = Player.instance.movementX;
            movementY = Player.instance.movementY;

            Vector2 toTarget = (target.position - transform.position).normalized;
            myRb.velocity = toTarget * movementSpeed;
        }
        else
        {
            myRb.velocity = Vector2.zero;
        }

        myAnim.SetFloat("moveX", myRb.velocity.x);
        myAnim.SetFloat("moveY", myRb.velocity.y);

        if (movementX == 1 || movementX == -1 || movementY == 1 || movementY == -1)
        {
            myAnim.SetFloat("lastMoveX", movementX);
            myAnim.SetFloat("lastMoveY", movementY);
        }
    }
}

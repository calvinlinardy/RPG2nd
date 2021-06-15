using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [HideInInspector] public float movementSpeed;
    public float walkingSpeed = 4f;
    public float runningSpeed = 6f;
    [HideInInspector] public float movementX = 0;
    [HideInInspector] public float movementY = 0;
    public bool canMove = true;

    //Cache references
    Rigidbody2D myRb;
    Animator myAnim;
    TeleportPoint teleportPoint;
    AudioSource audioSrc;
    public static Player instance;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

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

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementSpeed = runningSpeed;
        }
        else
        {
            movementSpeed = walkingSpeed;
        }

        if (canMove)
        {
            myRb.velocity = new Vector2(movementX, movementY).normalized * movementSpeed;
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                if (!audioSrc.isPlaying)
                {
                    audioSrc.Play();
                }
            }
            else
                audioSrc.Stop();
        }
        else
        {
            myRb.velocity = Vector2.zero;
            audioSrc.Stop();
        }

        myAnim.SetFloat("moveX", myRb.velocity.x);
        myAnim.SetFloat("moveY", myRb.velocity.y);

        if (movementX == 1 || movementX == -1 || movementY == 1 || movementY == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", movementX);
                myAnim.SetFloat("lastMoveY", movementY);
            }
        }
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        Player.instance.transform.position = GameObject.Find(teleportPoint.spawnPointName).transform.position;
        UIFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
    }
}

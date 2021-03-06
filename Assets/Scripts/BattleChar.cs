﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool isPlayer;
    public string[] movesAvailable;


    public string charName;
    public int currentHp, maxHp, currentMP, maxMP, strength, defence, weaponPower, armorPower;
    //public bool hasDied;

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

    Animator myAnim;

    private bool shouldFade;
    public float fadeSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {
            //theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime),
            //Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime),
            //Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            //if (theSprite.color.a == 0)
            //{
            gameObject.SetActive(false);
            //}
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }

    public void SetDieAnimation()
    {
        myAnim.SetTrigger("hasDied");
        myAnim.ResetTrigger("awaken");
    }

    public void SetAliveAnimation()
    {
        myAnim.SetTrigger("awaken");
        myAnim.ResetTrigger("hasDied");
    }
}

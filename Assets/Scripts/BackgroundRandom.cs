using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRandom : MonoBehaviour
{
    public Sprite[] backgroundList;
    private bool hasRandomized;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BattleManager.instance.battleActive && hasRandomized == false)
        {
            RandomizeBG();
            hasRandomized = true;
        }
        if (BattleManager.instance.battleActive == false)
        {
            hasRandomized = false;
        }
    }

    public void RandomizeBG()
    {
        GetComponent<SpriteRenderer>().sprite = backgroundList[Random.Range(0, backgroundList.Length)];
    }
}

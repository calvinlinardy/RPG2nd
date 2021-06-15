using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public float minRespawnRate = 15f;
    public float maxRespawnRate = 60f;
    public int minGold = 10;
    public int maxGold = 200;
    private int goldValue;
    private float timer;
    private int SFx = 15;
    private SpriteRenderer theSprite;
    private BoxCollider2D theCollider;
    private bool coinMissing = false;
    // Start is called before the first frame update
    void Start()
    {
        theSprite = GetComponentInChildren<SpriteRenderer>();
        theCollider = GetComponent<BoxCollider2D>();
        timer = Random.Range(minRespawnRate, maxRespawnRate);
        goldValue = Random.Range(minGold, maxGold);
    }

    // Update is called once per frame
    void Update()
    {
        if (coinMissing == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                coinMissing = false;
                theSprite.enabled = true;
                theCollider.enabled = true;
                timer = Random.Range(minRespawnRate, maxRespawnRate);
                goldValue = Random.Range(minGold, maxGold);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //coinnya ilang
            coinMissing = true;
            theSprite.enabled = false;
            theCollider.enabled = false;


            GameManager.instance.currentGold += goldValue;

            AudioManager.instance.PlaySFX(SFx);
        }
    }

    public IEnumerator WaitAfterSec(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}

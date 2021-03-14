using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlaManager : MonoBehaviour
{
    public static BattlaManager instance;
    // Start is called before the first frame update
    private bool battleActive;
    public GameObject battleScene;
    public Transform[] playerPos;
    public Transform[] enemyPos;
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Tortoise", "Zomb", "Slime" });
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!battleActive)
        {
            battleActive = true;
            GameManager.instance.battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x,
            Camera.main.transform.position.y, transform.position.z); //buat bikin BG ngikutin camera
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            for (int i = 0; i < playerPos.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPLayer = Instantiate(playerPrefabs[j], playerPos[i].position,
                            playerPos[i].rotation);
                            newPLayer.transform.parent = playerPos[i];
                            activeBattlers.Add(newPLayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHp = thePlayer.currentHP;
                            activeBattlers[i].maxHp = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].weaponPower = thePlayer.weaponPower;
                            activeBattlers[i].armorPower = thePlayer.armorPower;
                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPos[i].position,
                            enemyPos[i].rotation);
                            newEnemy.transform.parent = enemyPos[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    // Start is called before the first frame update
    public bool battleActive;
    public GameObject battleScene;
    public Transform[] playerPos;
    public Transform[] enemyPos;
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;
    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;

    public Text[] playerName, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public GameObject itemMenu;
    public BattleItemSelect[] itemButtons;
    public Item activeItem;
    public Text itemName, itemDescription, useButtonText;
    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;
    public CharStats[] playerStats;
    public GameObject itemActionWindow;
    public int chanceToFlee = 35;

    public string gameOverScene;

    private bool itemShown = false;
    private bool fleeing;
    public int rewardEXP;
    public string[] rewardItems;
    public bool cannotFlee;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        int battleManagerCount = FindObjectsOfType<BattleManager>().Length;
        if (battleManagerCount > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BattleStart(new string[] { "Carrot" }, false); //sementara
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    //musuh nya attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
            /*if (Input.GetKeyDown(KeyCode.N)) //Sementara
            {
                NextTurn();
            }*/
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if (!battleActive)
        {
            cannotFlee = setCannotFlee;

            battleActive = true;
            GameManager.instance.battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x,
            Camera.main.transform.position.y, transform.position.z); //buat bikin BG ngikutin camera
            battleScene.SetActive(true);

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
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count); //turnnya jadi random, kalo gamau random ya 0
            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
        UpdateStatsToGM();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHp < 0)
            {
                activeBattlers[i].currentHp = 0;
            }

            if (activeBattlers[i].currentHp <= 0)
            {
                //handle dead battler
                if (activeBattlers[i].isPlayer)
                {
                    //activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                    activeBattlers[i].SetDieAnimation();
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    //activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }

            if (activeBattlers[i].currentHp != 0)
            {
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].SetAliveAnimation();
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //game over
                Application.Quit();
                Debug.Log("Quit Game");
            }
            /*
            battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;*/
        }
        else
        {
            while (activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }
    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];

        //activeBattlers[selectedTarget].currentHp -= 30; //BUAT NGETEST

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position,
                activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, Quaternion.identity);
        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].weaponPower;
        float defPower = activeBattlers[target].defence + activeBattlers[target].armorPower;

        float damageCalc = (atkPower / defPower) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + " (" +
        damageToGive + ") damage to " + activeBattlers[target].charName); //sementara cuma buat liat di console

        activeBattlers[target].currentHp -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, Quaternion.identity).SetDamage(damageToGive);
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerName.Length; i++)
        {
            if (activeBattlers.Count > 1)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHp;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    //BUTTON GA BISA NERIMA FUNC YANG BUTUH INPUT 2 VAR
    {
        //int selectedTarget = 2;

        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position,
                activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, Quaternion.identity);
        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);
        itemMenu.SetActive(false);

        List<int> enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                enemies.Add(i);
            }
        }
        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (enemies.Count > i && activeBattlers[enemies[i]].currentHp > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = enemies[i];
                targetButtons[i].targetName.text = activeBattlers[enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);
        itemMenu.SetActive(false);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        itemMenu.SetActive(false);
        if (cannotFlee)
        {
            battleNotice.theText.text = "Cannot flee this battle!";
            battleNotice.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                //end battlenya
                /*
                battleActive = false;
                battleScene.SetActive(false);*/
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.theText.text = "Couldn't escape!";
                battleNotice.Activate();
            }
        }
    }

    public void ShowItem()
    {
        if (itemShown == false)
        {
            itemActionWindow.SetActive(false);
            itemName.text = "";
            itemDescription.text = "";
            itemShown = true;
            itemMenu.SetActive(true);
            GameManager.instance.SortItems();

            for (int i = 0; i < itemButtons.Length; i++)
            {
                itemButtons[i].buttonValue = i;
                if (GameManager.instance.itemsHeld[i] != "")
                {
                    itemButtons[i].buttonImage.gameObject.SetActive(true);
                    itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                    itemButtons[i].amountText.text = GameManager.instance.numberOfItem[i].ToString();
                }
                else
                {
                    itemButtons[i].buttonImage.gameObject.SetActive(false);
                    itemButtons[i].amountText.text = "";
                }
            }
        }
        else
        {
            itemMenu.SetActive(false);
            itemShown = false;
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);
        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void Useitem(int selectChar)
    {
        activeItem.Use(selectChar);
        UpdateStatsFromGM(selectChar);
        UpdateUIStats();
        CloseItemCharChoice();
        itemMenu.SetActive(false);
        NextTurn();
    }

    public void UpdateStatsFromGM(int selectChar)
    {
        if (activeBattlers[selectChar].isPlayer)
        {
            activeBattlers[selectChar].currentHp = GameManager.instance.playerStats[selectChar].currentHP;
            activeBattlers[selectChar].currentMP = GameManager.instance.playerStats[selectChar].currentMP;
            activeBattlers[selectChar].maxHp = GameManager.instance.playerStats[selectChar].maxHP;
            activeBattlers[selectChar].maxMP = GameManager.instance.playerStats[selectChar].maxMP;
            activeBattlers[selectChar].strength = GameManager.instance.playerStats[selectChar].strength;
            activeBattlers[selectChar].defence = GameManager.instance.playerStats[selectChar].defence;
            activeBattlers[selectChar].weaponPower = GameManager.instance.playerStats[selectChar].weaponPower;
            activeBattlers[selectChar].armorPower = GameManager.instance.playerStats[selectChar].armorPower;
        }
    }

    public void PlayButtonSound(int SFx)
    {
        AudioManager.instance.PlaySFX(SFx);
    }

    public void UpdateStatsToGM()
    {
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHp;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                        GameManager.instance.playerStats[j].maxHP = activeBattlers[i].maxHp;
                        GameManager.instance.playerStats[j].maxMP = activeBattlers[i].maxMP;
                        GameManager.instance.playerStats[j].strength = activeBattlers[i].strength;
                        GameManager.instance.playerStats[j].defence = activeBattlers[i].defence;
                        GameManager.instance.playerStats[j].weaponPower = activeBattlers[i].weaponPower;
                        GameManager.instance.playerStats[j].armorPower = activeBattlers[i].armorPower;
                    }
                }
            }
        }
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHp;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                        GameManager.instance.playerStats[j].maxHP = activeBattlers[i].maxHp;
                        GameManager.instance.playerStats[j].maxMP = activeBattlers[i].maxMP;
                        GameManager.instance.playerStats[j].strength = activeBattlers[i].strength;
                        GameManager.instance.playerStats[j].defence = activeBattlers[i].defence;
                        GameManager.instance.playerStats[j].weaponPower = activeBattlers[i].weaponPower;
                        GameManager.instance.playerStats[j].armorPower = activeBattlers[i].armorPower;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;

        if (fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardScreen(rewardEXP, rewardItems);
        }
        AudioManager.instance.StopMusic(3);
        AudioManager.instance.PlayBGM(musicBox.instance.musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);

        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        //Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene("Main Menu");
        UIFade.instance.FadeFromBlack();
    }
}

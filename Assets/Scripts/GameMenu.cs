using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    private CharStats[] playerStats;
    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatsHolder;
    public GameObject[] windows;
    public GameObject[] statusButtons;
    public GameObject itemActionWindow, buyActionWindow, sellActionWindow;
    public Text statusName, statusHP, statusMP, statusStr, statusDef,
    statusWpnEqpd, statusWpnPwr, statusAmrEqpd, statusAmrPwr, statusExp;
    public Image statusImg;

    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDesc, useButtonText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceName;
    public static GameMenu instance;
    public Text goldText, goldTextBig;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    private void Awake()
    {
        int gameMenuCount = FindObjectsOfType<GameMenu>().Length;
        if (gameMenuCount > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && BattleManager.instance.battleActive == false && !DialogManager.instance.dialogBox.activeInHierarchy
        && BattleReward.instance.rewardScreenOpened == false)
        {
            if (theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;
                CloseMenu();
            }
            else
            {
                if (!Shop.instance.shopMenu.activeInHierarchy)
                {
                    theMenu.SetActive(true);
                    UpdateMainStats();
                    GameManager.instance.gameMenuOpen = true;
                }
            }
            AudioManager.instance.PlaySFX(11);
        }
        UpdateMainStats();
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatsHolder[i].SetActive(true);
                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i].text = "" + playerStats[i].currentExp + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentExp;
                charImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                charStatsHolder[i].SetActive(false);
            }
        }
        goldText.text = GameManager.instance.currentGold.ToString();
        goldTextBig.text = GameManager.instance.currentGold.ToString();
    }

    public void ToggleWindows(int windowsNumber)
    {
        UpdateMainStats();
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowsNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;

        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();
        StatusChar(0);
        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defence.ToString();
        if (playerStats[selected].equippedWeapon != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWeapon;
        }
        statusWpnPwr.text = playerStats[selected].weaponPower.ToString();
        if (playerStats[selected].equippedArmor != "")
        {
            statusAmrEqpd.text = playerStats[selected].equippedArmor;
        }
        statusAmrPwr.text = playerStats[selected].armorPower.ToString();
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] -
        playerStats[selected].currentExp).ToString();
        statusImg.sprite = playerStats[selected].charImage;
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.
                GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItem[i].ToString();
            }
            else
            {
                //itemButtons[i].gameObject.SetActive(false);
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
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
        itemDesc.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharacterChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceName.Length; i++)
        {
            itemCharChoiceName[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceName[i].transform.parent.gameObject.SetActive
            (GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }
    public void CloseItemCharacterChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectedChar)
    {
        activeItem.Use(selectedChar);
        CloseItemCharacterChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void Work()
    {
        GameManager.instance.currentGold += 1;
        UpdateMainStats();
    }

    public void PlayButtonSound(int SFx)
    {
        AudioManager.instance.PlaySFX(SFx);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

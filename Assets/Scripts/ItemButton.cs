using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Press()
    {
        /*if (GameMenu.instance.theMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }

        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            if (Shop.instance.buyMenu.activeInHierarchy)
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }

            if (Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }*/
        if (GameManager.instance.itemsHeld[buttonValue] != "")
        {
            if (GameMenu.instance.theMenu.activeInHierarchy)
            {
                GameMenu.instance.itemActionWindow.SetActive(true);
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }
        else
        {
            GameMenu.instance.itemActionWindow.SetActive(false);
            GameMenu.instance.itemName.text = "";
            GameMenu.instance.itemDesc.text = "";
        }

        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            if (Shop.instance.itemsForSale[buttonValue] != "")
            {
                if (Shop.instance.buyMenu.activeInHierarchy)
                {
                    GameMenu.instance.buyActionWindow.SetActive(true);
                    Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
                }
            }
            else
            {
                GameMenu.instance.buyActionWindow.SetActive(false);
                Shop.instance.buyItemName.text = "";
                Shop.instance.buyItemDesc.text = "";
                Shop.instance.buyItemValue.text = "";
            }

            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                if (Shop.instance.sellMenu.activeInHierarchy)
                {
                    GameMenu.instance.sellActionWindow.SetActive(true);
                    Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
                }
            }
            else
            {
                GameMenu.instance.sellActionWindow.SetActive(false);
                Shop.instance.sellItemName.text = "";
                Shop.instance.sellItemDesc.text = "";
                Shop.instance.sellItemValue.text = "";
            }
        }
    }
}

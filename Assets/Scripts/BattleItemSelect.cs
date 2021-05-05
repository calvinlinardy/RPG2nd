using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemSelect : MonoBehaviour
{
    public string itemName;
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Press()
    {
        if (BattleManager.instance.itemMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                BattleManager.instance.itemActionWindow.SetActive(true);
                BattleManager.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
            else
            {
                BattleManager.instance.itemActionWindow.SetActive(false);
                BattleManager.instance.itemName.text = "";
                BattleManager.instance.itemDescription.text = "";
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEditor.Progress;
using TMPro;
using System;

// Shop script
public class ShopManager : MonoBehaviour
{
    public int coins;
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemsSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;

    void Start()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
        coinUI.text = "Coins : " + coins.ToString();
        LoadPanels();
        CheckPurchasable();
    }
    void Update()
    {
        
    }
    public void AddCoins()
    {
        coins += 100;
        coinUI.text = "Coins : " + coins.ToString();
        CheckPurchasable();
    }
    public void CheckPurchasable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if(coins >= shopItemsSO[i].baseCost)
            {
                myPurchaseBtns[i].interactable = true;
            } else
            {
                myPurchaseBtns[i].interactable = false;
            }
        }
    }
    public void PurchaseItem(int btnNo) {
        if (coins >= shopItemsSO[btnNo].baseCost)
        {
            coins -= shopItemsSO[btnNo].baseCost;
            coinUI.text = "Coins : " + coins.ToString();
            CheckPurchasable();
        }
    }
    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsSO[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsSO[i].description;
            shopPanels[i].costTxt.text = shopItemsSO[i].baseCost.ToString() + " coins";
        }
    }
}
using System;
using Cloud.DataBaseSystem.UserData;
using Player_Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int _itemId;
    [SerializeField] private int _itemCost;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private TextMeshProUGUI _buttonText;
    
    private void Start()
    {
        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        _buttonText.color = UserDataHandler.Singleton.UserData.GoldValue < _itemCost ? Color.red : Color.white;
    }

    public void PurchaseItem()
    {
        if (UserDataHandler.Singleton.UserData.GoldValue >= _itemCost)
        {
            PlayerNetCode.Singleton.CharacterInventory.AddItemToDesiredSlot(_itemId, 1, 0);
            UserDataHandler.Singleton.AddGold(-_itemCost);
            UpdateButtonState();
        }
    }
}
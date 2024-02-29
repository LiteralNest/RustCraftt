using Cloud.DataBaseSystem.UserData;
using TMPro;
using UnityEngine;

public class ShopCurrencyAddTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;
    

    private void Start()
    {
        UpdateCurrencyText();
    }

    public void AddGold(int amount)
    {
        UserDataHandler.Singleton.UserData.GoldValue += amount;
        UpdateCurrencyText();
    }
    
    private void UpdateCurrencyText()
    {
        if (_currencyText != null)
        {
            _currencyText.text = UserDataHandler.Singleton.UserData.GoldValue.ToString();
        }
    }
}
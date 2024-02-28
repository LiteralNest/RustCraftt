using TMPro;
using UnityEngine;

public class ShopCurrencyAddTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;
    private int _goldAmount = 0;

    private void Start()
    {
        UpdateCurrencyText();
    }

    public void AddGold(int amount)
    {
        _goldAmount += amount;
        UpdateCurrencyText();
    }
    
    private void UpdateCurrencyText()
    {
        if (_currencyText != null)
        {
            _currencyText.text = _goldAmount.ToString();
        }
    }
}
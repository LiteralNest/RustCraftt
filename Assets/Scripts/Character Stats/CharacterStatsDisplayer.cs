using System.Threading.Tasks;
using Alerts_System.Alerts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsDisplayer : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Image _hpFill;
    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private Image _foodFill;
    [SerializeField] private TMP_Text _waterText;
    [SerializeField] private Image _waterFill;
    [SerializeField] private TextMeshProUGUI _displayMessage;
    [SerializeField] private TextMeshProUGUI _oxygenText;

    [SerializeField] private Image _oxygenFill;
    // private void Awake()
    // {
    //     GlobalEventsContainer.PlayerSpawned += HideDeathMessage;
    // }

    public void DisplayHp(int hp)
    {
        if (_hpText != null)
            _hpText.text = hp.ToString();
        if (_hpFill != null)
            _hpFill.fillAmount = (float)hp / 100;
    }

    public void DisplayFood(int food)
    {
        float fixedfood = food;
        if (food < 0)
        {
            fixedfood = 0;
            AlertsDisplayer.Singleton.DisplayStarvingAlert(true);
        }
        else
            AlertsDisplayer.Singleton.DisplayStarvingAlert(false);
        _foodText.text = fixedfood.ToString();
        _foodFill.fillAmount = fixedfood / 100;
    }

    public void DisplayWater(int water)
    {  
        float fixedwater = water;
        if (water < 0)
        {
            AlertsDisplayer.Singleton.DisplayDehydratedAlert(true);
            fixedwater = 0;
        }
        else
            AlertsDisplayer.Singleton.DisplayDehydratedAlert(false);
        _waterText.text = fixedwater.ToString();
        _waterFill.fillAmount = fixedwater / 100;
    }


    public void DisplayOxygen(int oxygen)
    {
        _oxygenText.text = oxygen.ToString();
        _oxygenFill.fillAmount = (float)oxygen / 100;
    }

    public async void DisplayDeathMessage(string message, Color color)
    {
        _displayMessage.text = message;
        _displayMessage.color = color;
        _displayMessage.alpha = 1f;

        await Task.Delay(5000);

        _displayMessage.alpha = 0f;
    }

    // public void HideDeathMessage()
    // {
    //     _displayMessage.alpha = 0f;
    // }
    // private void OnDisable()
    // {
    //     GlobalEventsContainer.PlayerSpawned -= HideDeathMessage;
    // }
}
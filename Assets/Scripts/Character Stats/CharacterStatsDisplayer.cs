using System.Threading.Tasks;
using Alerts_System.Alerts;
using Player_Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character_Stats
{
    public class CharacterStatsDisplayer : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private TMP_Text _hpText;
        [SerializeField] private Image _hpFill;
        [SerializeField] private TMP_Text _foodText;
        [SerializeField] private Image _foodFill;
        [SerializeField] private TMP_Text _waterText;
        [SerializeField] private Image _waterFill;
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
                PlayerNetCode.Singleton.AlertsDisplayer.DisplayStarvingAlert(true);
            }
            else
                PlayerNetCode.Singleton.AlertsDisplayer.DisplayStarvingAlert(false);
            _foodText.text = fixedfood.ToString();
            _foodFill.fillAmount = fixedfood / 100;
        }

        public void DisplayWater(int water)
        {  
            float fixedwater = water;
            if (water < 0)
            {
                PlayerNetCode.Singleton.AlertsDisplayer.DisplayDehydratedAlert(true);

                fixedwater = 0;
            }
            else
                PlayerNetCode.Singleton.AlertsDisplayer.DisplayDehydratedAlert(false);
            _waterText.text = fixedwater.ToString();
            _waterFill.fillAmount = fixedwater / 100;
        }


        public void DisplayOxygen(int oxygen)
        {
            _oxygenText.text = oxygen.ToString();
            _oxygenFill.fillAmount = (float)oxygen / 100;
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
}
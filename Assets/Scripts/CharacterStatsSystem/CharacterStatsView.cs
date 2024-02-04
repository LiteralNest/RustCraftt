using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterStatsSystem
{
    public class CharacterStatsView : MonoBehaviour
    {
        [Header("Hp")] [SerializeField] private TMP_Text _hpText;
        [SerializeField] private Image _hpFill;
        [Header("Food")] [SerializeField] private TMP_Text _foodText;
        [SerializeField] private Image _foodFill;
        [Header("Water")] [SerializeField] private TMP_Text _waterText;
        [SerializeField] private Image _waterFill;
        [Header("Oxygen")] [SerializeField] private TextMeshProUGUI _oxygenText;
        [SerializeField] private Image _oxygenFill;

        private void OnEnable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign += Init;

        private void OnDisable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign -= Init;

        private void Init(CharacterStats stats)
        {
            stats.Hp.OnValueChanged += (int oldValue, int newValue) => DisplayStat(_hpText, _hpFill, newValue);
            stats.Food.OnValueChanged += (int oldValue, int newValue) => DisplayStat(_foodText, _foodFill, newValue);
            stats.Water.OnValueChanged += (int oldValue, int newValue) => DisplayStat(_waterText, _waterFill, newValue);
            stats.Oxygen.OnValueChanged +=
                (int oldValue, int newValue) => DisplayStat(_oxygenText, _oxygenFill, newValue);
        }

        private void DisplayStat(TMP_Text text, Image fill, int value)
        {
            text.text = value.ToString();
            fill.fillAmount = (float)value / 100;
            
            //Display Alert
        }
    }
}
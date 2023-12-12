using System.Collections.Generic;
using Armor_System.UI;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ResistsDisplayer : MonoBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private List<ArmorSlotDisplayer> _slotsDisplayers;
        
        [Header("UI")] [SerializeField] private TMP_Text _radiationResistText;
        [SerializeField] private TMP_Text _hitResistText;
        [SerializeField] private TMP_Text _temperatureResistText;
        [SerializeField] private TMP_Text _explosionResistText;
        
        private void DisplayResist(int value, TMP_Text text)
            => text.text = value.ToString() + "%";
        
        
        public void DisplayValues()
        {
            int radiationResistValue = 0;
            int hitResistValue = 0;
            int temperatureResistValue = 0;
            int explosionResistValue = 0;

            foreach (var slotDisplayer in _slotsDisplayers)
            {
                var armor = slotDisplayer.GetCurrentArmor();
                if(armor == null) continue;
                radiationResistValue += armor.RadiationResistValue;
                hitResistValue += armor.HitResistValue;
                temperatureResistValue += armor.TemperatureResistValue;
                explosionResistValue += armor.ExplosionResistValue;
            }

            DisplayResist(radiationResistValue, _radiationResistText);
            DisplayResist(hitResistValue, _hitResistText);
            DisplayResist(temperatureResistValue, _temperatureResistText);
            DisplayResist(explosionResistValue, _explosionResistText);
        }
    }
}
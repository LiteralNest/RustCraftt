using CharacterStatsSystem;
using Events;
using InHandItems.InHandViewSystem;
using UnityEngine;
using UnityEngine.UI;

namespace InHandItems
{
    public class HealView : InHandView
    {
        [SerializeField] private Button _healButton;

        private CharacterStats _characterStats;
        
        private void Init(CharacterStats characterStats)
        {
            _characterStats = characterStats;
            characterStats.Hp.OnValueChanged += (int oldValue, int newValue) => TryDisplayHealButton(newValue);
        }

        private void TryDisplayHealButton(int value)
            => _healButton.gameObject.SetActive(value < 100);

        public override void Init(IViewable viewable)
        {
            var heal = viewable as InHandHeal;
            _healButton.onClick.AddListener(heal.Heal);
            _healButton.onClick.AddListener(() => GlobalEventsContainer.OnActiveSlotReset?.Invoke());
        }

        public void DisplayHealButton(bool value)
        {
            if (value && _characterStats)
                TryDisplayHealButton(_characterStats.Hp.Value);
            else
                _healButton.gameObject.SetActive(false);
        }
    }
}
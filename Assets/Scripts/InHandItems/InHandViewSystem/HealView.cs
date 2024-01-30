using Events;
using Player_Controller;
using UnityEngine;
using UnityEngine.UI;

namespace InHandItems.InHandViewSystem
{
    public class HealView : InHandView
    {
        [SerializeField] private Button _healButton;

        private void OnEnable()
            => GlobalEventsContainer.CharacterHpChanged += TryDisplayHealButton;
        
        private void OnDisable()
            => GlobalEventsContainer.CharacterHpChanged -= TryDisplayHealButton;

        private void TryDisplayHealButton()
        {
            var hp = PlayerNetCode.Singleton.CharacterHpHandler.Hp;
            _healButton.gameObject.SetActive(hp < 100);
        }

        public override void Init(IViewable viewable)
        {
            var heal = viewable as InHandHeal;
            _healButton.onClick.AddListener(heal.Heal);
        }

        public void DisplayHealButton(bool value)
        {
            if (value)
                TryDisplayHealButton();
            else
                _healButton.gameObject.SetActive(false);
        }
    }
}
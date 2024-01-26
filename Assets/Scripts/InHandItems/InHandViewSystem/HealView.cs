using UnityEngine;
using UnityEngine.UI;

namespace InHandItems.InHandViewSystem
{
    public class HealView : InHandView
    {
        [SerializeField] private Button _healButton;

        public override void Init(IViewable viewable)
        {
            var heal = viewable as InHandHeal;
            _healButton.onClick.AddListener(heal.Heal);
        }
        
        public void DisplayHealButton(bool value)
            => _healButton.gameObject.SetActive(value);
    }
}
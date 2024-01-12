using FightSystem.Weapon.WeaponTypes;
using UnityEngine;
using UnityEngine.UI;

namespace InHandItems.InHandViewSystem
{
    public class LongRangeInHandView : InHandView
    {
        [Header("UI")] 
        [SerializeField] private CustomButton _attackButton;
        [SerializeField] private Button _scopeButton;
        [SerializeField] private Button _reloadButton;

        public override void Init(IViewable viewable)
        {
            var weapon = viewable as BaseShootingWeapon;
            
            AssignAttack(false);
            
            _attackButton.PointerDown.AddListener(() =>
            {
                weapon.HandleShoot(true);
            });
            
            _attackButton.PointerClicked.AddListener(() =>
            {
                weapon.HandleShoot(false);
            });

            AssignScope(true);
            _scopeButton.onClick.AddListener(weapon.Scope);

            weapon.TryDisplayReload();
            _reloadButton.onClick.AddListener(weapon.Reload);
        }

        private void AssignScope(bool value)
            => _scopeButton.gameObject.SetActive(value);
        
        public void AssignReload(bool value)
            => _reloadButton.gameObject.SetActive(value);
        
        public void AssignAttack(bool value)
            => _attackButton.gameObject.SetActive(value);
    }
}
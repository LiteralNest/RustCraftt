using FightSystem.Weapon.Melee;
using UnityEngine;

namespace InHandViewSystem
{
    public class ThrowingInHandView : InHandView
    {
        [SerializeField] private CustomButton _scopeButton;
        [SerializeField] private CustomButton _attackButton; 

        public override void Init(IViewable weapon)
        {
            var throwingWeapon = weapon as MeleeShootingWeapon;

            _scopeButton.PointerDown.AddListener(() => { throwingWeapon.SetThrowingPosition(true); });
            _scopeButton.PointerClickedWithoudDisable.AddListener(() => { throwingWeapon.SetThrowingPosition(false); });
            _attackButton.PointerDown.AddListener(() => { throwingWeapon.SetAttack(true); });
            _attackButton.PointerClickedWithoudDisable.AddListener(() => { throwingWeapon.SetAttack(false); });
            
            DisplayScopeButton(true);
            DisplayAttackButton(true);
        }
        
        public void DisplayScopeButton(bool value) => _scopeButton.gameObject.SetActive(value);
        public void DisplayAttackButton(bool value) => _attackButton.gameObject.SetActive(value);
    }
}
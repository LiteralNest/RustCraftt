using FightSystem.Weapon.Melee;
using UI;
using UnityEngine;

namespace InHandItems.InHandViewSystem
{
    public class ThrowingInHandView : InHandView
    {
        [SerializeField] private CustomButton _scopeButton;
        [SerializeField] private CustomButton _attackButton;

        public override void Init(IViewable weapon)
        {
            var throwingWeapon = weapon as MeleeShootingWeapon;

            _scopeButton.PointerDown.AddListener(() =>
            {
                throwingWeapon.SetScope(); 
                DisplayAttackButton(false);
            });
            _scopeButton.PointerClickedWithoudDisable.AddListener(() =>
            {
                throwingWeapon.SetThrow();
                DisplayAttackButton(true);
            });
            _attackButton.PointerDown.AddListener(() => { throwingWeapon.SetAttack(true); });
            _attackButton.PointerClickedWithoudDisable.AddListener(() => { throwingWeapon.SetAttack(false); });
            
            DisplayScopeButton(true);
            DisplayAttackButton(true);
        }

        public void DisplayScopeButton(bool value) => _scopeButton.gameObject.SetActive(value);
        public void DisplayAttackButton(bool value) => _attackButton.gameObject.SetActive(value);
    }
}
using FightSystem.Weapon.ShootWeapon;
using UI;
using UnityEngine;

namespace InHandItems.InHandViewSystem
{
    public class BowInHandView: InHandView
    {
        [SerializeField] private CustomButton _attackButton;

        public override void Init(IViewable viewable)
        {
            var weapon = viewable as Bow;
            _attackButton.PointerDown.AddListener(() => { weapon.Scope(); });
            _attackButton.PointerClicked.AddListener(() => { weapon.Attack(); });
        }
    }
}
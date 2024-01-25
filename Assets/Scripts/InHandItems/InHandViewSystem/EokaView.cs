using FightSystem.Weapon.WeaponTypes;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace InHandItems.InHandViewSystem
{
    public class EokaView : InHandView
    {
        [SerializeField] private CustomButton _attackButton;
        [SerializeField] private Button _scopeButton;

        public override void Init(IViewable weapon)
        {
            var eoka = weapon as RandomShotgunWeapon;
            _attackButton.PointerDown.AddListener(() => { eoka.StartAttack(); });
            _attackButton.PointerClicked.AddListener(() => {eoka.StopAttack(); });
            _scopeButton.onClick.AddListener(eoka.Scope);
        }
    }
}

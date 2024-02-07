using FightSystem.Weapon.WeaponTypes;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace InHandItems.InHandViewSystem
{
    public class EokaView : InHandView
    {
        [SerializeField] private CustomButton _attackButton;
        [SerializeField] private Button _reloadButton;

        public override void Init(IViewable weapon)
        {
            var eoka = weapon as RandomShotgunWeapon;
            _attackButton.PointerDown.AddListener(() => { eoka.StartAttack(); });
            _attackButton.PointerClickedWithoudDisable.AddListener(() => {eoka.StopAttack(); });
            _reloadButton.onClick.AddListener(eoka.Reload);
        }
        
        public void DisplayReloadButton(bool value) => _reloadButton.gameObject.SetActive(value);
        public void DisplayAttackButton(bool value) => _attackButton.gameObject.SetActive(value);
    }
}

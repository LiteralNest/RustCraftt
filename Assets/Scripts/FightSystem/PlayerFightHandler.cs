using Events;
using FightSystem.Weapon.ShootWeapon;
using UI;
using UnityEngine;

namespace FightSystem
{
    public class PlayerFightHandler : MonoBehaviour
    {
        private BaseShootingWeapon _currentBaseShootingWeapon;
        private bool _attacking;
        private void OnEnable()
            => GlobalEventsContainer.WeaponObjectAssign += AssignWeaponObject;
    
        private void OnDisable()
            => GlobalEventsContainer.WeaponObjectAssign -= AssignWeaponObject;
    
        private void Update()
        {

        }

        private void AssignWeaponObject(BaseShootingWeapon value)
        {
            _currentBaseShootingWeapon = value;
            if (_currentBaseShootingWeapon == null) return;
            _currentBaseShootingWeapon.Init();
        }
        

        public void Reload()
        {
            if(!_currentBaseShootingWeapon) return;
            _currentBaseShootingWeapon.Reload();
            CharacterUIHandler.singleton.ActivateReloadingButton(false);
        }

        public void Scope()
        {
            if(!_currentBaseShootingWeapon) return;
            _currentBaseShootingWeapon.Scope();
        }
    }
}
using Events;
using UI;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class MeleeShootingWeapon : MonoBehaviour
    {
        [Header("ThrowingObject")] [SerializeField]
        private WeaponThrower _weaponThrower;

        [SerializeField] private GameObject _mainObj;
        private Vector3 _direction;
        private bool _wasScoped;

        private void OnEnable()
        {
            CharacterUIHandler.singleton.ActivateMeleeThrowButton(true);
            GlobalEventsContainer.WeaponMeleeObjectAssign?.Invoke(this);
            _wasScoped = false;
        }

        private void OnDisable()
        {
            CharacterUIHandler.singleton.ActivateMeleeThrowButton(false);
            if (_wasScoped) return;
            GlobalEventsContainer.WeaponMeleeObjectAssign?.Invoke(null);
        }

        public void SetThrowingPosition(bool value)
        {
            _wasScoped = true;
            _mainObj.SetActive(false);
            _weaponThrower.gameObject.SetActive(value);
            if (value) return;
            _weaponThrower.ThrowSpear();
            gameObject.SetActive(false);
        }

        protected void Attack(bool value)
            => GlobalEventsContainer.ShouldHandleAttacking?.Invoke(value);
    }
}
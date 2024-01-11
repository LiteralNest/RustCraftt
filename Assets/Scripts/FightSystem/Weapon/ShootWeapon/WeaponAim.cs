using FightSystem.Weapon.ShootWeapon.Sway;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class WeaponAim
    {
        private Transform _weaponAimPosition;
        private WeaponSway _weaponSway;
        private AimSway _aimSway;
        private ISway _currentSway;
        private Transform _transform;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private bool _isScoping;
        public bool IsAiming { get; private set; }
        

        public WeaponAim(Transform weaponAimPosition, Transform transform, WeaponSway weaponSway, AimSway aimSway)
        {
            _weaponAimPosition = weaponAimPosition;
            _transform = transform;
            _weaponSway = weaponSway;
            _aimSway = aimSway;
            _currentSway = _weaponSway;
            _currentSway.CanSway = true;
            _originalPosition = _transform.localPosition;
            _originalRotation = _transform.localRotation;
        }

        public void UpdateSway()
        {
            _currentSway.UpdateSway();
        }

        private void SetOnAimPosition()
        {
            _transform.position = _weaponAimPosition.position;
            _transform.localRotation = _weaponAimPosition.localRotation;

            _currentSway.CanSway = false;

            IsAiming = true;
        }

        private void SetOnOriginalPosition()
        {
            _transform.localPosition = _originalPosition;
            _transform.localRotation = _originalRotation;

            _currentSway.CanSway = false;

            IsAiming = false;
        }

        public void UnScope(out bool wasScoped)
        {
            wasScoped = false;
            if (_isScoping)
            {
                wasScoped = true;
                SetOnOriginalPosition();
            }
            _isScoping = false;
        }

        public void SetScope()
        {
            if (_isScoping)
                SetOnOriginalPosition();
            else
                SetOnAimPosition();
            _isScoping = !_isScoping;
        }
    }
}
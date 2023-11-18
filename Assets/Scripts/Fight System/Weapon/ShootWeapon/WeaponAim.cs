using UnityEngine;
using UnityEngine.Serialization;

namespace Fight_System.Weapon.ShootWeapon
{
    public class WeaponAim : MonoBehaviour
    {
        [SerializeField] private Transform _weaponAimPosition;
        [SerializeField] private WeaponSway _weaponSway;
        [FormerlySerializedAs("_weaponAimSway")] [SerializeField] private AimSway _aimSway;
        
        [SerializeField] private float _aimSpeed = 5f;

        private Transform _transform;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private bool _isScoping;
        public bool IsAiming { get; private set; }

        private void Awake()
        {
            _transform = transform;
            _originalPosition = _transform.localPosition;
            _originalRotation = _transform.localRotation;
        }

        private void SetOnAimPosition()
        {
            _transform.localPosition = _weaponAimPosition.localPosition;
            _transform.localRotation = _weaponAimPosition.localRotation;
            
            _weaponSway.enabled = false;
            _aimSway.enabled = true;

            IsAiming = true;
        }

        private void SetOnOriginalPosition()
        {
            _transform.localPosition = _originalPosition;
            _transform.localRotation = _originalRotation;

            _weaponSway.enabled = true;
            _aimSway.enabled = false;
            
            IsAiming = false;
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
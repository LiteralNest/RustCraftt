using Sirenix.OdinInspector;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class WeaponRecoil
    {
        private Vector3 _currentRotation;
        private Quaternion _originalRotation;
        private Transform _targetTransform;
        
        public WeaponRecoil(Transform transform)
        {
            _targetTransform = transform;
        }
        
        public void ApplyRecoil(float recoilX, float recoilY, float recoilZ)
        {
            _originalRotation = _targetTransform.localRotation;
            
            _currentRotation += new Vector3(recoilX, recoilY, recoilZ);
            
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -3f, 3f);
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, -5f, 3f);
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, -5f, 5f);
            
            _targetTransform.localRotation = new Quaternion(_currentRotation.x, _currentRotation.y, _currentRotation.z, 0.5f);
        }

        public void UpdateRecoil(float returnSpeed)
        {
            _targetTransform.localRotation = Quaternion.Slerp(_targetTransform.localRotation, _originalRotation, Time.deltaTime * returnSpeed);
        }
        
        [Button]
        public void ResetRecoil()
        {
            _originalRotation = Quaternion.identity;
            _currentRotation = Vector3.zero;
        }
    }
}
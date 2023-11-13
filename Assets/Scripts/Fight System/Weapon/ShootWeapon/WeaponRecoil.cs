using System;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class WeaponRecoil : MonoBehaviour
    {
        private Vector3 _currentRotation;
        private Quaternion _originalRotation;

        private void Start()
        {
            _originalRotation = transform.localRotation;
        }

   
        public void ApplyRecoil(float recoilX, float recoilY, float recoilZ)
        {
            _currentRotation += new Vector3(recoilX, recoilY, recoilZ);
            
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -3f, 3f);
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, -5f, 3f);
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, -5f, 5f);
            
            transform.localRotation = new Quaternion(_currentRotation.x, _currentRotation.y, _currentRotation.z, 0.5f);
            
            Debug.Log("x" +_currentRotation.x + " " + "y" + _currentRotation.y + " " + "z"+ _currentRotation.z);
        }

        public void UpdateRecoil(float returnSpeed)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, _originalRotation, Time.deltaTime * returnSpeed);
        }

        // Add this method to reset the recoil when needed
        public void ResetRecoil()
        {
            _currentRotation = Vector3.zero;
        }
    }
}
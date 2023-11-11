using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class WeaponRecoil : MonoBehaviour
    {
        private Vector3 _currentRotation;

        public void ApplyRecoil(float recoilX, float recoilY, float recoilZ)
        {
            _currentRotation += new Vector3(recoilX, recoilY, recoilZ);
        }

        public void UpdateRecoil(float returnSpeed)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
        }
    }
}
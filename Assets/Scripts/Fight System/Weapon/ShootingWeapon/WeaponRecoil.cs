using UnityEngine;
using Random = UnityEngine.Random;

namespace Fight_System.Weapon.ShootingWeapon
{
    public class WeaponRecoil : MonoBehaviour
    {
        private Vector3 currentRotation;

        public void ApplyRecoil(float recoilX, float recoilY, float recoilZ)
        {
            currentRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }

        public void UpdateRecoil(float returnSpeed)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
        }
    }
}
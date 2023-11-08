using UnityEngine;

namespace Fight_System.Weapon.ShootingWeapon
{
    public class WeaponSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _shotClip;
        [SerializeField] private AudioSource _shotSource;

        public void PlayShot() => _shotSource.PlayOneShot(_shotClip);
    }
}
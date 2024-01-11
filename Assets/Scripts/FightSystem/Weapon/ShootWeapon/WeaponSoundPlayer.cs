using Items_System.Items.Weapon;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class WeaponSoundPlayer
    {
        private AudioClip _shotClip;
        private AudioSource _shotSource;

        public WeaponSoundPlayer(AudioSource shotSource, ShootingWeapon riffleWeapon)
        {
            _shotClip = riffleWeapon.ShotClip;
            _shotSource = shotSource;
        }

        public void PlayShot() => _shotSource.PlayOneShot(_shotClip);
    }
}
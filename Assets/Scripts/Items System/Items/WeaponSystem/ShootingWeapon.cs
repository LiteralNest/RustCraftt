using UnityEngine;

namespace Items_System.Items.WeaponSystem
{
    [CreateAssetMenu(menuName = "Item/Shooting Weapon")]
    [System.Serializable]
    public class ShootingWeapon : Weapon
    {
        [Header("Shooting Weapon")] 
        [SerializeField] private float _range = 100;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _delayBetweenShoots = 0.1f;
        [SerializeField] private Ammo _ammo;
        [SerializeField] private int _magazineCount;
        [SerializeField] private float _recoilX;
        [SerializeField] private float _recoilY;
        [SerializeField] private float _recoilZ;
        [SerializeField] private AudioClip _shotClip;

        public float Range => _range;
        public int Damage => _damage;
        public float DelayBetweenShoots => _delayBetweenShoots;
        public Ammo Ammo => _ammo;
        public int MagazineCount => _magazineCount;
        public float RecoilX => _recoilX;
        public float RecoilY => _recoilY;
        public float RecoilZ => _recoilZ;
        public AudioClip ShotClip => _shotClip;
    }
}
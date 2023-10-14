using UnityEngine;

[CreateAssetMenu(menuName = "Item/Long Range Weapon")]
[System.Serializable]
public class LongRangeWeapon : Weapon
{
    public float Range => _range;
    [Header("Long Range Weapon")] 
    [SerializeField] private float _range = 100;
    public int Damage => _damage;
    [SerializeField] private int _damage = 1;
    public float DelayBetweenShoots => _delayBetweenShoots;
    [SerializeField]private float _delayBetweenShoots = 0.1f;
    
    public Ammo Ammo => _ammo;
    [Header("Ammo")]
    [SerializeField] private Ammo _ammo;
    public int MagazineCount => _magazineCount;
    [SerializeField] private int _magazineCount;
    
    public float RecoilX => _recoilX;
    [Header("Recoil")] 
    [SerializeField] private float _recoilX;
    public float RecoilY => _recoilY;
    [SerializeField] private float _recoilY;
    public float RecoilZ => _recoilZ;
    [SerializeField] private float _recoilZ;
    public float ReturnSpeed => _returnSpeed;
    [SerializeField] private float _returnSpeed;
    public float Snappiness => _snappiness;
    [SerializeField] private float _snappiness;
}
using UnityEngine;

[RequireComponent(typeof(WeaponSoundPlayer))]
public class RiffleWeapon : BaseShootingWeapon
{
    [Header("In Game Init")] 
    private int startingAmmoCount = 100;

    private LongRangeWeaponInventoryItemDisplayer inventoryItemDisplayer;
    
    private void Start()
    {
        Reload();
        canShoot = true;
        currentAmmoCount = startingAmmoCount;
    }

    private void Update()
    {
        Recoil.UpdateRecoil();
    }

    protected void Attack(bool value)
    {
        if (!CanShoot() || currentAmmoCount <= 0) return;
        RaycastHit hit;
        SoundPlayer.PlayShot();
        MinusAmmo();
        Recoil.ApplyRecoil(Weapon.RecoilX, Weapon.RecoilY, Weapon.RecoilZ);
        StartCoroutine(DisplayFlameEffect()); // Start the coroutine
        if (Physics.Raycast(transform.position, transform.forward, out hit, Weapon.Range, TargetMask))
        {
            TryDamage(hit);
            DisplayHit(hit);
        }
        StartCoroutine(WaitBetweenShootsRoutine());
    }
}
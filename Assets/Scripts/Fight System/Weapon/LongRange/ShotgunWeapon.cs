using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(WeaponSoundPlayer))]
public class ShotgunWeapon : BaseShootingWeapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private int _pelletCount = 12;
    [SerializeField] private float _spreadAngle = 20f;

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
        Recoil.UpdateRecoil(2f);
    }

    [ContextMenu("Shot")]
    private void TestShot()
        => Attack(true);

    public override void Attack(bool value)
    {
        if (!CanShoot() || currentAmmoCount <= 0) return;
        SoundPlayer.PlayShot();
        MinusAmmo();
        SpreadShots(); // Need to add logic of scope
        StartCoroutine(WaitBetweenShootsRoutine());
    }

    private void SpreadShots()
    {
        var spawnPoint = AmmoSpawnPoint.position;
        var shootDirection = transform.forward;

        for (int i = 0; i < _pelletCount; i++)
        {
            var spreadWorldPos = Random.insideUnitCircle * _spreadAngle;

            var spreadOffset = new Vector3(spreadWorldPos.x, spreadWorldPos.y, 0f);

            var shootRay = new Ray(spawnPoint, shootDirection + spreadOffset);

            Recoil.ApplyRecoil(Weapon.RecoilX, Weapon.RecoilY, Weapon.RecoilZ);;

            if (Physics.Raycast(shootRay, out var hit, Weapon.Range, TargetMask))
            {
                TryDamage(hit);
                DisplayHit(hit);
            }
        }
    }
}
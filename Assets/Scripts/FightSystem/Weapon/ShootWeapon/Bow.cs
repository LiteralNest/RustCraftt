using System.Collections;
using Building_System.NetWorking;
using FightSystem.Weapon.ShootWeapon.Ammo;
using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Inventory_System;
using Items_System.Items.Abstract;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class Bow : MonoBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/BowWeaponView";

        [Header("Bow")] [SerializeField] private Items_System.Items.Ammo _ammo;
        [SerializeField] private float _arrowForce;
        [SerializeField] private BowAnimator _weaponAnimator;
        [SerializeField] private Transform _ammoSpawnPoint;
        [SerializeField] private AnimationClip _prepearingForShootClip;
        [SerializeField] private Item _targetItem;
        private Arrow _currentArrow;
        private Vector3 _force;
        
        private bool _canShoot;

        private int _currentAmmoCount;

        private BowInHandView _inHandView;

        private void Start()
        {
            _inHandView = Instantiate(Resources.Load<BowInHandView>(ViewName), transform);
            _inHandView.Init(this);
        }

        public void Attack()
        {
            if (_currentAmmoCount <= 0) return;
            if (!_canShoot)
            {
                _weaponAnimator.SetIdle();
                return;
            }
            _weaponAnimator.SetAttack();
            ShootArrow();
            InventoryHandler.singleton.CharacterInventory.RemoveItem(_ammo.Id, 1);
        }

        public void Scope()
        {
            if (!EnoughAmmo()) return;
            StartCoroutine(WaitForScopingRoutine());
            _currentAmmoCount = 1;
            _weaponAnimator.SetScope();
        }
        
        private IEnumerator WaitForScopingRoutine()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_prepearingForShootClip.length);
            _canShoot = true;
        }

        private bool EnoughAmmo()
        {
            var ammoCount = InventoryHandler.singleton.CharacterInventory.GetItemCount(_ammo.Id);
            return ammoCount > 0;
        }

        private void ShootArrow()
        {
            AmmoObjectsPool.Singleton.SpawnArrowServerRpc(_targetItem.Id,_ammoSpawnPoint.position, _ammoSpawnPoint.rotation, transform.eulerAngles.x - 90);
            _currentAmmoCount--;
        }
    }
}
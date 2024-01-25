using System;
using Building_System.NetWorking;
using FightSystem.Weapon.ShootWeapon.Ammo;
using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class Bow : MonoBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/BowWeaponView";

        [Header("Bow")] [SerializeField] private Items_System.Items.Ammo _ammo;
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _arrowForce;
        [SerializeField] private BowAnimator _weaponAnimator;
        [SerializeField] private Transform _ammoSpawnPoint;
        private Arrow _currentArrow;
        private Vector3 _force;

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
            _weaponAnimator.SetAttack();
            ShootArrow();
            InventoryHandler.singleton.CharacterInventory.RemoveItem(_ammo.Id, 1);
        }

        public void Scope()
        {
            if (!EnoughAmmo()) return;
            _currentAmmoCount = 1;
            _weaponAnimator.SetScope();
        }

        private bool EnoughAmmo()
        {
            var ammoCount = InventoryHandler.singleton.CharacterInventory.GetItemCount(_ammo.Id);
            return ammoCount > 0;
        }

        private void ShootArrow()
        {
            _force = _ammoSpawnPoint.TransformDirection(Vector3.forward * _arrowForce);
            AmmoObjectsPool.Singleton.SpawnArrowServerRpc(_ammoSpawnPoint.position, _ammoSpawnPoint.rotation, _force);
            _currentAmmoCount--;
        }
    }
}
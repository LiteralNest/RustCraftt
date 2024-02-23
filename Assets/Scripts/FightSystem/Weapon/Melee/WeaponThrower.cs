using FightSystem.Weapon.Ballistic;
using Inventory_System;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class WeaponThrower : NetworkBehaviour
    {
        [Header("Attached Compontents")] [SerializeField]
        private Transform _spawnPoint;

        [SerializeField] private ThrowingWeapon _targetPref;
        [Header("Physics")] [SerializeField] private float _throwForce = 40f;

        private BallisticCalculator _ballisticCalculator = new();

        private bool _startedThrow;

        [ServerRpc(RequireOwnership = false)]
        private void SpawnSpearServerRpc(Vector3 spawnPoint, Quaternion rotation, float angle, int spearHp)
        {
            if (!IsServer) return;
            var target = Instantiate(_targetPref, spawnPoint, rotation);
            target.GetComponent<NetworkObject>().Spawn();
            target.Throw(angle, spearHp);
        }

        private void ThrowSpear()
        {
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer == null) return;

            var hp = 100;
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer != null)
                hp = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Hp;

            var position = _spawnPoint.position;
            var angle = _ballisticCalculator.GetCalculatedAngle(_spawnPoint.forward, _throwForce);
            SpawnSpearServerRpc(position, _spawnPoint.rotation, angle, hp);

            InventoryHandler.singleton.CharacterInventory.RemoveItem(
                InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);
        }

        public void StartThrow()
        {
            _startedThrow = true;
        }

        public void EndThrow()
        {
            Debug.Log("Try Throw");
            if (!_startedThrow) return;
            Debug.Log("Throw");
            _startedThrow = false;
            ThrowSpear();
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
        }
    }
}
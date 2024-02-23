using Inventory_System;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class WeaponThrower : NetworkBehaviour
    {
        [Header("Attached Compontents")] 
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ThrowingWeapon _targetPref;
        [Header("Physics")] 
        [SerializeField] private float _throwForce = 40f;
        
        private bool _startedThrow;
        
        [ServerRpc(RequireOwnership = false)]
        private void SpawnSpearServerRpc(Vector3 spawnPoint, Quaternion rotation,float angle ,int spearHp)
        {
            if(!IsServer) return;
            var target = Instantiate(_targetPref, spawnPoint, rotation);
            target.GetComponent<NetworkObject>().Spawn();
            target.Throw(spearHp, transform.forward, angle);
        }
        
 

        private void ThrowSpear()
        {
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer == null) return;

            var hp = 100;
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer != null)
                hp = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Hp;

            var angle = CalculateAngle();
            SpawnSpearServerRpc(_spawnPoint.position, _spawnPoint.rotation, angle, hp);

            InventoryHandler.singleton.CharacterInventory.RemoveItem(
                InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);
        }

        private float CalculateAngle()
        {
            var bowDirection = transform.forward;

            var bowTiltAngle = Vector3.Angle(Vector3.up, bowDirection) - 90f;
            var deltaY = Mathf.Clamp(bowTiltAngle, 0f, 90f);

            var angle = Mathf.Atan2(deltaY, _throwForce) * Mathf.Rad2Deg;
            return angle;
        }
        public void StartThrow()
        {
            _startedThrow = true;
        }
        
        public void EndThrow()
        {
            Debug.Log("Try Throw");
            if(!_startedThrow) return;
            Debug.Log("Throw");
            _startedThrow = false;
            ThrowSpear();
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
        }
    }
}
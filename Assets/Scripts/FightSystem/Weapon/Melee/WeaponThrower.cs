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
        private ThrowingWeapon _target;

        [ServerRpc(RequireOwnership = false)]
        private void SpawnSpearServerRpc(Vector3 direction, Vector3 spawnPoint, Quaternion rotation, int spearHp)
        {
            _target = Instantiate(_targetPref, spawnPoint, rotation);
            _target.GetComponent<NetworkObject>().Spawn();
            _target.Throw(direction, _throwForce, spearHp);
        }

        private void ThrowSpear()
        {
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer == null) return;

            var hp = 100;
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer != null)
                hp = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Hp;
            SpawnSpearServerRpc(Camera.main.transform.forward, _spawnPoint.position, _spawnPoint.rotation, hp);

            InventoryHandler.singleton.CharacterInventory.RemoveItem(
                InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);
        }

        public void EndThrow()
        {
            ThrowSpear();
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
        }
    }
}
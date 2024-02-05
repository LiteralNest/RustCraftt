using System.Collections;
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
        [SerializeField] private AnimationClip _throwingAnim;

        [Header("Physics")] [SerializeField] private float _throwForce = 40f;
        private ThrowingWeapon _target;

        [ServerRpc(RequireOwnership = false)]
        private void SpawnSpearServerRpc(Vector3 direction, Vector3 spawnPoint, Quaternion rotation)
        {
            _target = Instantiate(_targetPref, spawnPoint, rotation);
            _target.GetComponent<NetworkObject>().Spawn();
            _target.Throw(direction, _throwForce);
        }

        public void ThrowSpear()
        {
            if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer == null) return;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(
                InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);
            SpawnSpearServerRpc(Camera.main.transform.forward, _spawnPoint.position, _spawnPoint.rotation);

            StartCoroutine(ThrowRoutine());
        }

        private IEnumerator ThrowRoutine()
        {
            yield return new WaitForSeconds(_throwingAnim.length);
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
        }
    }
}
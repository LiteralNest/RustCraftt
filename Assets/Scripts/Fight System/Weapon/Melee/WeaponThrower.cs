using System;
using Events;
using UI;
using Unity.Netcode;
using UnityEngine;

public class WeaponThrower : NetworkBehaviour
{
    [Header("Attached Compontents")] [SerializeField]
    private Transform _spawnPoint;

    [SerializeField] private ThrowingWeapon _targetPref;

    [Header("Physics")] [SerializeField] private float _throwForce = 40f;

    private ThrowingWeapon _target;

    private void OnEnable()
    {
        CharacterUIHandler.singleton.ActivateAttackButton(false);
    }

    private void OnDisable()
    {
        CharacterUIHandler.singleton.ActivateMeleeThrowButton(false);
        GlobalEventsContainer.WeaponMeleeObjectAssign?.Invoke(null);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnSpearServerRpc(Vector3 direction, Vector3 spawnPoint, Quaternion rotation)
    {
        _target = Instantiate(_targetPref, spawnPoint, rotation);
        _target.GetComponent<NetworkObject>().Spawn();
        _target.Throw(direction, _throwForce);
    }

    private void SpawnSpear(Transform spawnPoint)
    {
        SpawnSpearServerRpc(Camera.main.transform.forward, spawnPoint.position, spawnPoint.rotation);
        //Дописати неткод
    }

    public void ThrowSpear()
    {
        if (InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer == null) return;
        InventoryHandler.singleton.CharacterInventory.RemoveItem(
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);
        SpawnSpear(_spawnPoint);
        gameObject.SetActive(false);
    }
}
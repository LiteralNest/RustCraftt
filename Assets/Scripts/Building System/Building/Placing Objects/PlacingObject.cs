using System.Collections.Generic;
using Building_System.Building.Blocks;
using Building_System.Upgrading;
using Inventory_System;
using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Placing_Objects
{
    public class PlacingObject : BuildingStructure, IHammerInteractable
    {
        [field: SerializeField] public Item TargetItem { get; private set; }
        public NetworkVariable<int> OwnerId { get; set; } = new();

        private PlacingObjectDamageHandler _damageHandler;
        private IPlacingObjectInteractable _interactable;

        public PlacingObjectDamageHandler DamageHandler => _damageHandler;

        private void Awake()
            => _interactable = GetComponent<IPlacingObjectInteractable>();

        private void Start()
            => _damageHandler = GetComponent<PlacingObjectDamageHandler>();

        public void SetOwnerId(int id)
        {
            OwnerId.Value = id;
            _interactable?.Init(id);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyObjectServerRpc()
        {
            if (!IsServer) return;
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }

        #region IHammerInteractable

        public bool CanBePickUp()
            => true;

        public void PickUp()
        {
            InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(TargetItem.Id, 1, 0);
            DestroyObjectServerRpc();
        }

        public InventoryCell GetNeededItemsForUpgrade()
        {
            throw new System.NotImplementedException();
        }

        public bool CanBeUpgraded(int level)
            => false;

        public List<InventoryCell> GetNeededCellsForUpgrade(int level)
        {
            throw new System.NotImplementedException();
        }

        public void UpgradeTo(int level)
        {
            throw new System.NotImplementedException();
        }

        public int GetLevel()
        {
            throw new System.NotImplementedException();
        }

        public bool CanBeRepaired()
            => false;

        public void Repair()
        {
            throw new System.NotImplementedException();
        }

        public bool CanBeDestroyed()
            => false;

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
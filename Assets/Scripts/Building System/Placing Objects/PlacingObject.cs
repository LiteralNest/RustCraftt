using Building_System.Blocks;
using Building_System.Upgrading;
using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace Building_System.Placing_Objects
{
    public class PlacingObject : BuildingStructure, IHammerInteractable
    {
        [field: SerializeField] public Item TargetItem { get; private set; }
        public NetworkVariable<int> OwnerId { get; set; } = new();
        private IPlacingObjectInteractable _interactable;

        private void Awake()
            => _interactable = GetComponent<IPlacingObjectInteractable>();

        [ServerRpc(RequireOwnership = false)]
        public void SetOwnerIdServerRpc(int id)
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

        public bool CanBeUpgraded()
            => false;

        public void Upgrade()
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
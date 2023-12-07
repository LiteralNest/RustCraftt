using Building_System.Blocks;
using Building_System.Upgrading;
using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Placing_Objects
{
    public class PlacingObject : BuildingStructure, IHammerInteractable
    {
        [field: SerializeField] public Item TargetItem { get; private set; }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyObjectServerRpc()
        {
            if(!IsServer) return;
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
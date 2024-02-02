using InteractSystem;
using Inventory_System;
using Items_System.Items.Abstract;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Items_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootingItem : NetworkBehaviour, IRaycastInteractable
    {
        public NetworkVariable<CustomSendingInventoryDataCell> Data { get; set; } = new();
        [field: SerializeField] public Item TargetItem { get; private set; }
        
        public string GetDisplayText()
            => ItemFinder.singleton.GetItemById(Data.Value.Id).Name;

        public void Interact()
        {
           InventoryHandler.singleton.CharacterInventory.AddItemToSlotWithAlert(Data.Value.Id,
               Data.Value.Count, Data.Value.Ammo, Data.Value.Hp);
           PickUpServerRpc();
        }

        public bool CanInteract()
            => true;

        [ServerRpc(RequireOwnership = false)]
        private void PickUpServerRpc()
        {
            if (IsServer)
                GetComponent<NetworkObject>().Despawn();
        }
    }
}
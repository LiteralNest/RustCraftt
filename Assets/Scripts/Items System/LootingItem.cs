using System.Collections;
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
        [SerializeField] private NetworkObject _targetNetworkObject;
        [field: SerializeField] public Item TargetItem { get; private set; }
        public NetworkVariable<CustomSendingInventoryDataCell> Data { get; set; } = new();

        private void Awake()
        {
            if (_targetNetworkObject == null)
                _targetNetworkObject = GetComponent<NetworkObject>();
        }

        public string GetDisplayText()
            => ItemFinder.singleton.GetItemById(Data.Value.Id).Name;

        public void Init(CustomSendingInventoryDataCell data)
        {
            Data.Value = data;
            StartCoroutine(DespawnRoutine());
        }

        public void InitByTargetItem()
        {
            Data.Value = new CustomSendingInventoryDataCell(TargetItem.Id, 1, 0, 0);
            StartCoroutine(DespawnRoutine());
        }

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
                _targetNetworkObject.Despawn();
        }

        private IEnumerator DespawnRoutine()
        {
            var item = ItemFinder.singleton.GetItemById(Data.Value.Id);
            yield return new WaitForSeconds(item.DestroySecondsTime);
            _targetNetworkObject.Despawn();
        }
    }
}
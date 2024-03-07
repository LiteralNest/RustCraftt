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
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private NetworkObject _targetNetworkObject;
        [field: SerializeField] public Item TargetItem { get; private set; }
        public NetworkVariable<CustomSendingInventoryDataCell> Data { get; set; } = new(new CustomSendingInventoryDataCell(-1, 0, 0, 0));

        private void Awake()
        {
            if (_targetNetworkObject == null)
                _targetNetworkObject = GetComponent<NetworkObject>();
        }

        public string GetDisplayText()
        {
            if(Data.Value.Id == -1) return "";
            return ItemFinder.singleton.GetItemById(Data.Value.Id).Name;
        }

        public void Init(CustomSendingInventoryDataCell data)
        {
            StartCoroutine(InitWithDelayRoutine(data));
            StartCoroutine(DespawnRoutine());
        }

        private IEnumerator InitWithDelayRoutine(CustomSendingInventoryDataCell data)
        {
            yield return new WaitForSeconds(1f);
            Data.Value = data;
        }

        public void InitByTargetItem(int hp = 0, bool shouldDelete = false)
        {
            if (shouldDelete)
                _targetNetworkObject.Despawn();
            Data.Value = new CustomSendingInventoryDataCell(TargetItem.Id, 1, hp, 0);
            StartCoroutine(DespawnRoutine());
        }

        public void Interact()
        {
            InventoryHandler.singleton.CharacterInventory.AddItemToSlotWithAlert(Data.Value.Id,
                Data.Value.Count, Data.Value.Ammo, Data.Value.Hp);
            PickUpServerRpc();
        }

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanDisplayInteract()
            => true;

        public bool CanInteract()
            => Data.Value.Id != -1;

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
using System.Collections;
using InteractSystem;
using Inventory_System;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public class StorageBag : NetworkBehaviour, IRaycastInteractable
    {
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Storage _targetStorage;

        private void OnEnable()
        {
            if(!IsServer) return;
            _targetStorage.ItemsNetData.OnValueChanged += (_, _) =>
            {
                if (InventoryEmpty(_targetStorage.ItemsNetData.Value))
                    _networkObject.Despawn();
            };
            StartCoroutine(DespawnRoutine());
        }

        public string GetDisplayText()
            => "Open";

        public void Interact()
            => _targetStorage.Interact();

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
            => true;

        private bool InventoryEmpty(CustomSendingInventoryData data)
        {
            foreach(var slot in data.Cells)
                if(slot.Id != -1)
                    return false;
            return true;
        }
        
        public bool CanDisplayInteract()
            => true;
        
        private IEnumerator DespawnRoutine()
        {
            yield return new WaitForSeconds(1200f);
            _networkObject.Despawn();
        }
    }
}
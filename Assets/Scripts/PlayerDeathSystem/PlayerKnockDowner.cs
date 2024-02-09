using System.Threading.Tasks;
using Animation_System;
using CharacterStatsSystem;
using Events;
using InteractSystem;
using Inventory_System;
using Multiplayer;
using Player_Controller;
using Player_Controller.Looking_Around;
using UI;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace PlayerDeathSystem
{
    public class PlayerKnockDowner : NetworkBehaviour, IRaycastInteractable
    {
        public static PlayerKnockDowner Singleton { get; private set; }
        [SerializeField] private Sprite _displayIcon;
        [Header("Head")] [SerializeField] private PlayerRotator _playerRotator;


        private NetworkVariable<bool> _knockDown = new();
        public bool IsKnockDown => _knockDown.Value;

        private async void Start()
        {
            await Task.Delay(1000);
            if (!IsOwner) return;
            Singleton = this;
        }

        #region KnockDown

        [ServerRpc(RequireOwnership = false)]
        public void KnockDownServerRpc(int id)
        {
            if (!IsServer) return;
            _knockDown.Value = true;
            KnockDownClientRpc(id);
        }

        [ClientRpc]
        private void KnockDownClientRpc(int id)
        {
            if (UserDataHandler.Singleton.UserData.Id == id)
            {
                GlobalEventsContainer.OnPlayerKnockDown?.Invoke();
                _playerRotator.SetKnockDownHead();
                GetComponent<PlayerController>().enabled = false;
                MainUiHandler.Singleton.DisplayKnockDownScreen(true);
                AnimationsManager.Singleton.SetKnockDown();

                if (InventoryHandler.singleton.ActiveSlotDisplayer == null) return;
                var cellIndex = InventoryHandler.singleton.ActiveSlotDisplayer.Index;
                var data = InventoryHandler.singleton.CharacterInventory.ItemsNetData.Value.Cells[cellIndex];

                var slotDisplayer = InventoryHandler.singleton.ActiveSlotDisplayer;
                if (!(slotDisplayer == null || slotDisplayer.ItemDisplayer == null || slotDisplayer.ItemDisplayer.InventoryCell.Item == null)){
                          InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(data,
                                    Camera.main.transform.position + Camera.main.transform.forward);
                                InventoryHandler.singleton.CharacterInventory.RemoveItem(slotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);
                }
     
                PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
                InventoryHandler.singleton.ActiveSlotDisplayer = null;
            }
        }

        [ContextMenu("KnockDown")]
        private void KnockDown()
        {
            KnockDownServerRpc(UserDataHandler.Singleton.UserData.Id);
        }

        #endregion

        #region StandUp

        [ServerRpc(RequireOwnership = false)]
        public void StandUpServerRpc()
        {
            if (!IsServer) return;
            _knockDown.Value = false;
            StandUpClientRpc();
        }

        [ClientRpc]
        private void StandUpClientRpc()
        {
            AnimationsManager.Singleton.SetIdle();
            if (IsOwner)
            {
                GetComponent<PlayerController>().enabled = true;
                MainUiHandler.Singleton.DisplayKnockDownScreen(false);
                GlobalEventsContainer.OnPlayerStandUp?.Invoke();
                CharacterStatsEventsContainer.OnCharacterStatAdded.Invoke(CharacterStatType.Health, 10);
                _playerRotator.SetDefaultHead();
            }
        }

        [ContextMenu("StandUp")]
        private void StandUp()
        {
            StandUpServerRpc();
        }

        #endregion

        public string GetDisplayText()
            => "Help";

        public void Interact()
            => StandUpServerRpc();

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
        {
            if (!_knockDown.Value && IsOwner) return false;
            return true;
        }
    }
}
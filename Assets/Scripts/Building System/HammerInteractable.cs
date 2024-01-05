using Building_System.Placing_Objects;
using Building_System.Upgrading;
using Tool_Clipboard;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace Building_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class HammerInteractable : NetworkBehaviour, IHammerInteractable
    {
        [SerializeField] private PlacingObject _targetPlacingObject;

        public ToolClipboard TargetToolClipboard
        {
            get => _targetToolClipboard;
            set => _targetToolClipboard = value;
        }

        private ToolClipboard _targetToolClipboard;

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

        public bool CanBePickUp()
        {
            if (_targetToolClipboard == null) return true;
            return _targetToolClipboard.IsAutorized(UserDataHandler.singleton.UserData.Id);
        }

        public void PickUp()
        {
            InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(
                _targetPlacingObject.TargetItem.Id, 1, 0);
            DestroyObjectServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyObjectServerRpc()
        {
            if (!IsServer) return;
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
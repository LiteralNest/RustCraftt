using InteractSystem;
using Lock_System;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Doors_System
{
    public class DoorHandler : NetworkBehaviour, ILockable, IRaycastInteractable
    {
        [SerializeField] private Animator _anim;
        private Locker _locker;

        private NetworkVariable<bool> _wasOpened = new();
        private NetworkVariable<int> _activeLock = new(0);
        private NetworkVariable<bool> _canBeInteracted = new(true);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _wasOpened.OnValueChanged += (bool prevValue, bool newValue) => { OpenClientRpc(newValue); };
        }

        private void Open(int id)
        {
            if (_locker != null && !_locker.CanBeOpened(id)) return;
            OpenServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void OpenServerRpc()
        {
            if (!IsServer) return;
            _canBeInteracted.Value = false;
            _wasOpened.Value = !_wasOpened.Value;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetCanBeInteractedServerRpc()
        {
            if (!IsServer) return;
            _canBeInteracted.Value = true;
        }

        [ClientRpc]
        private void OpenClientRpc(bool value)
        {
            if (value)
                _anim.SetTrigger("Open");
            else
                _anim.SetTrigger("Close");
        }

        #region IRaycastInteractable

        public string GetDisplayText()
        {
            if (_wasOpened.Value)
                return "Close";
            return "Open";
        }

        public void Interact()
            => Open(UserDataHandler.Singleton.UserData.Id);

        public bool CanInteract()
            => _canBeInteracted.Value;

        #endregion


        #region ILockable

        public void Lock(Locker locker)
            => _locker = locker;

        bool ILockable.IsLocked()
            => _locker != null;

        #endregion
    }
}
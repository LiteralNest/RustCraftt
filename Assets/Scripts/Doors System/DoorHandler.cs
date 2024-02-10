using InteractSystem;
using Lock_System;
using Sound_System;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Doors_System
{
    public class DoorHandler : NetworkBehaviour, ILockable, IRaycastInteractable
    {
        [SerializeField] private Sprite _openDoorIcon;
        [SerializeField] private Sprite _closeDoorIcon;
        [SerializeField] private Animator _anim;
        
        [Header("Sound")]
        [SerializeField] private NetworkSoundPlayer _networkSoundPlayer;
        [SerializeField] private AudioClip _openClip;
        [SerializeField] private AudioClip _closeClip;
        
        private Locker _locker;

        private NetworkVariable<bool> _wasOpened = new();
        private NetworkVariable<bool> _canBeInteracted = new(true);

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
            Open(_wasOpened.Value);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetCanBeInteractedServerRpc()
        {
            if (!IsServer) return;
            _canBeInteracted.Value = true;
        }

        private void Open(bool value)
        {
            if (value)
            {
                _networkSoundPlayer.PlayOneShot(_openClip);
                _anim.SetTrigger("Open");
            }
            else
            {
                _networkSoundPlayer.PlayOneShot(_closeClip);
                _anim.SetTrigger("Close");
            }
        }

        #region IRaycastInteractable

        public string GetDisplayText()
        {
            if (_locker != null && !_locker.AvailableForOpen(UserDataHandler.Singleton.UserData.Id)) return "Locked";
            if (_wasOpened.Value)
                return "Close";
            return "Open";
        }

        public void Interact()
            => Open(UserDataHandler.Singleton.UserData.Id);

        public Sprite GetIcon()
        {
            if (_wasOpened.Value) return _closeDoorIcon;
            return _openDoorIcon;
        }

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
using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using InHandItems.InHand;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace OnPlayerItems
{
    public class InHandObjectsContainer : NetworkBehaviour
    {
        [SerializeField] private List<InHandObjectCell> _inHandObjects;
        [SerializeField] private PlayerNetCode _playerNetCode;
        [SerializeField] private InHandObject _defaultHands;
        private InHandObjectCell _currentCell;

        private void Awake()
        {
            AssignDefaultHands();
        }

        private void OnEnable()
        {
            GlobalEventsContainer.ShouldHandleAttacking += HandleAttacking;
            GlobalEventsContainer.OnCurrentItemDeleted += ResetCurrentCell;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.ShouldHandleAttacking -= HandleAttacking;
            GlobalEventsContainer.OnCurrentItemDeleted -= ResetCurrentCell;
        }

        private async void Start()
        {
            await Task.Delay(1100);
            SetDefaultHands();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            SetDefaultHands();
        }

        private void ResetCurrentCell()
        {
            SetDefaultHands();
            _currentCell = null;
        }

        public void DisplayItems(int itemId)
        {
            bool isOwner = _playerNetCode.IsOwner;
            foreach (var obj in _inHandObjects)
                obj.ActivateInHandObject(isOwner, false);

            foreach (var obj in _inHandObjects)
            {
                if (obj.TargetItem.Id == itemId)
                {
                    obj.ActivateInHandObject(isOwner);
                    _currentCell = obj;
                    return;
                }
            }
        }

        public void SetWalk(bool value)
        {
            if (_currentCell == null) return;
            _currentCell.SetWalk(value);
        }

        public void SetRun(bool value)
        {
            if (_currentCell == null) return;
            _currentCell.SetRun(value);
        }

        public void SetDefaultHands()
            => DisplayItems(-1);

        private void HandleAttacking(bool attack)
        {
            if (!_playerNetCode.IsOwner) return;
            if (_currentCell == null) return;
            _currentCell.HandleAttacking(attack);
        }

        private void AssignDefaultHands()
        {
            foreach (var cell in _inHandObjects)
            {
                if (cell.ThirdPersonObject != null) continue;
                cell.ThirdPersonObject = _defaultHands;
            }
        }

        public void SetCrouch(bool value)
        {
            
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SetAttackAnimationServerRpc(bool value)
        {
            if(!IsServer) return;
            SetAttackAnimationClientRpc(value);
        }

        [ClientRpc]
        private void SetAttackAnimationClientRpc(bool value)
        {
            if (_currentCell == null)
            {
                Debug.LogWarning("Current cell is null");
                return;
            }
            _currentCell.HandleAttacking(value);
        }
    }
}
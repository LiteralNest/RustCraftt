using System.Collections.Generic;
using Events;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace OnPlayerItems
{
    public class InHandObjectsContainer : NetworkBehaviour
    {
        [SerializeField] private List<InHandObjectCell> _inHandObjects;
        [SerializeField] private PlayerNetCode _playerNetCode;
        private InHandObjectCell _currentCell;
        
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

        private void ResetCurrentCell()
        {
            _playerNetCode.SetDefaultHandsServerRpc();
            _currentCell = null;
        }

        public void DisplayItems(int itemId)
        {
            bool isOwner = _playerNetCode.IsOwner;
            if(isOwner && itemId == -1)
                GlobalEventsContainer.OnActiveSlotReset?.Invoke();
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

        private void HandleAttacking(bool attack)
        {
            if (!_playerNetCode.IsOwner) return;
            if (_currentCell == null) return;
            _currentCell.HandleAttacking(attack);
        }
    }
}
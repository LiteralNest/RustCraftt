using System.Collections.Generic;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

public class InHandObjectsContainer : NetworkBehaviour
{
    [SerializeField] private List<InHandObjectCell> _inHandObjects;
    [SerializeField] private PlayerNetCode _playerNetCode;
    [SerializeField] private InHandObject _defaultHands;
    private InHandObjectCell _currentCell;

    private void Awake()
        => AssignDefaultHands();
    
    private void OnEnable()
    {
        GlobalEventsContainer.ShouldHandleAttacking += HandleAttacking;
    }

    private void OnDisable()
    {
        GlobalEventsContainer.ShouldHandleAttacking -= HandleAttacking;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetDefaultHands();
    }

    public void DisplayItems(int itemId)
    {
        bool isOwner = _playerNetCode.PlayerIsOwner();
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
    
    [ContextMenu("Set Default Hands")]
    private void AssignDefaultHands()
    {
        foreach (var cell in _inHandObjects)
        {
            if (cell.ThirdPersonObject != null) continue;
            cell.ThirdPersonObject = _defaultHands;
        }
    }
}
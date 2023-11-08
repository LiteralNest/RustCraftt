using System.Collections.Generic;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

public class InHandObjectsContainer : NetworkBehaviour
{
    [SerializeField] private List<InHandObjectCell> _inHandObjects;
    [SerializeField] private PlayerNetCode _playerNetCode;
    private InHandObjectCell _currentCell;
    
    private void OnEnable()
    {
        GlobalEventsContainer.ShouldHandleAttacking += HandleAttacking;
        GlobalEventsContainer.ShouldHandleWalk += SetWalk;
        GlobalEventsContainer.ShouldHandleRun += SetRun;
    }
    
    private void OnDisable()
    {
        GlobalEventsContainer.ShouldHandleAttacking -= HandleAttacking;
        GlobalEventsContainer.ShouldHandleWalk -= SetWalk;
        GlobalEventsContainer.ShouldHandleRun -= SetRun;
    }

    private void Start()
        => SetDefaultHands();

    public void DisplayItems(int itemId)
    {
        bool isOwner = _playerNetCode.PlayerIsOwner();
        foreach (var obj in _inHandObjects)
        {
            if (obj.TargetItem.Id == itemId)
            {
                obj.ActivateInHandObject(isOwner);
                _currentCell = obj;
                continue;
            }

            obj.ActivateInHandObject(isOwner, false);
        }
    }

    public void SetWalk(bool value)
    {
        if(_currentCell == null) return;
        _currentCell.SetWalk(value);
    }

    public void SetRun(bool value)
    {
        if(_currentCell == null) return;
        _currentCell.SetRun(value);
    }

    public void SetDefaultHands()
        =>  DisplayItems(0);

    private void HandleAttacking(bool attack)
    {
        if(!_playerNetCode.IsOwner) return;
        if(_currentCell == null) return;
        _currentCell.HandleAttacking(attack);
    }
}
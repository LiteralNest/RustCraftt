using System.Collections.Generic;
using UnityEngine;

public class InHandObjectsContainer : MonoBehaviour
{
    [SerializeField] private List<InHandObjectCell> _inHandObjects;
    [SerializeField] private PlayerNetCode _playerNetCode;
    private InHandObjectCell _currentCell;

    private void Start()
    {
        TurnOffObject();
    }

    private void TurnOffObject()
    {
        foreach (var obj in _inHandObjects)
        {
            obj.FirstPersonObject.enabled = false;
            obj.ThirdPersonObject.enabled = false;
        }
    }
    
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

    public void HandleAttacking(bool attack)
    {
        if(_currentCell == null) return;
        _currentCell.HandleAttacking(attack);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class InHandObjectsContainer : MonoBehaviour
{
    [SerializeField] private List<InHandObjectCell> _inHandObjects;
    [SerializeField] private PlayerNetCode _playerNetCode;
    [SerializeField] private GameObject _defaultHands;
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
    {
        if(_playerNetCode.IsOwner)
            _defaultHands.SetActive(false);
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
        _defaultHands.SetActive(false);
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

    private void HandleAttacking(bool attack)
    {
        if(!_playerNetCode.IsOwner) return;
        if(_currentCell == null) return;
        _currentCell.HandleAttacking(attack);
    }
}
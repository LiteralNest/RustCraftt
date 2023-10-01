using System.Collections.Generic;
using UnityEngine;

public class InHandObjectsContainer : MonoBehaviour
{
    [SerializeField] private List<InHandObjectCell> _inHandObjects;
    [SerializeField] private PlayerNetCode _playerNetCode;
    public void DisplayItems(int itemId)
    {
        bool isOwner = _playerNetCode.PlayerIsOwner();
        foreach (var obj in _inHandObjects)
        {
            if (obj.TargetItem.Id == itemId)
            {
                obj.ActivateInHandObject(isOwner);
                continue;
            }
            obj.ActivateInHandObject(isOwner, false);
        }
    }
}
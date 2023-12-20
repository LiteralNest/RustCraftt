using System.Collections.Generic;
using Items_System.Items.Abstract;
using UnityEngine;

[System.Serializable]
public class InHandObjectCell
{
    [field: SerializeField] public Item TargetItem { get; private set; }
    [field: SerializeField] public InHandObject FirstPersonObject { get; set; }
    [field: SerializeField] public InHandObject ThirdPersonObject { get; set; }

    private void DisplayInHandItem(bool fpValue, bool tpValue)
    {
        if (FirstPersonObject != null)
            FirstPersonObject.DisplayRenderers(fpValue);
        if (ThirdPersonObject != null)
            ThirdPersonObject.DisplayRenderers(tpValue);
    }

    public void ActivateInHandObject(bool isOwner, bool shouldActivate = true)
    {
        if (!shouldActivate)
        {
            DisplayInHandItem(false, false);
            return;
        }

        if (isOwner)
            DisplayInHandItem(true, false);
        else
            DisplayInHandItem(false, true);
        }

    public void SetWalk(bool value)
    {
        if (FirstPersonObject)
            FirstPersonObject.Walk(value);
        if (ThirdPersonObject)
            ThirdPersonObject.Walk(value);
    }

    public void SetRun(bool value)
    {
        if (FirstPersonObject)
            FirstPersonObject.Run(value);
        if (ThirdPersonObject)
            ThirdPersonObject.Run(value);
    }

    public void HandleAttacking(bool attack)
    {
        if (FirstPersonObject)
            FirstPersonObject.HandleAttacking(attack);
        if (ThirdPersonObject)
            ThirdPersonObject.HandleAttacking(attack);
    }
}
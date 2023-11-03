using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InHandObjectCell
{
    [field: SerializeField] public Item TargetItem { get; private set; }
    [field: SerializeField] public InHandObject FirstPersonObject { get; private set; }
    [field: SerializeField] public InHandObject ThirdPersonObject { get; private set; }

    public void ActivateInHandObject(bool isOwner, bool shouldActivate = true)
    {
        if (!shouldActivate)
        {
            if (FirstPersonObject != null)
                FirstPersonObject.DisplayRenderers(false);
            if (ThirdPersonObject != null)
                ThirdPersonObject.DisplayRenderers(false);
            return;
        }

        if (isOwner)
        {
            FirstPersonObject.DisplayRenderers(true);
            ThirdPersonObject.DisplayRenderers(false);
        }
        else
        {
            FirstPersonObject.DisplayRenderers(false);
            ThirdPersonObject.DisplayRenderers(true);
        }
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
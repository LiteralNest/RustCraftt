using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InHandObjectCell
{
    [field: SerializeField] public Item TargetItem { get; private set; }
    [field: SerializeField] public InHandObject FirstPersonObject { get; private set; }
    [field: SerializeField] public InHandObject ThirdPersonObject { get; private set; }
    [field: SerializeField] public List<Renderer> Renderers { get; private set; }

    public void ActivateInHandObject(bool isOwner, bool shouldActivate = true)
    {
        if (!shouldActivate)
        {
            if (FirstPersonObject != null)
                FirstPersonObject.gameObject.SetActive(false);
            if (ThirdPersonObject != null)
                ThirdPersonObject.gameObject.SetActive(false);
            return;
        }

        // if (isOwner)
        // {
        //     if(FirstPersonObject != null)
        //         FirstPersonObject.gameObject.SetActive(true);
        // }
        // else
        // {
        //     if(ThirdPersonObject != null)
        //         ThirdPersonObject.gameObject.SetActive(true);
        // }

        if (FirstPersonObject != null)
            FirstPersonObject.gameObject.SetActive(true);
        // if (ThirdPersonObject != null)
        //     ThirdPersonObject.gameObject.SetActive(true);
    }
    
    private void DisableThirdPersonRenderers()
    {
        foreach (var renderer in Renderers)
            renderer.enabled = false;
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
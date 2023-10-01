using UnityEngine;

[System.Serializable]
public class InHandObjectCell
{
    [field: SerializeField] public Item TargetItem { get; private set; }
    [field: SerializeField] public GameObject FirstPersonObject { get; private set; }
    [field: SerializeField] public GameObject ThirdPersonObject { get; private set; }

    public void ActivateInHandObject(bool isOwner, bool shouldActivate = true)
    {
        if (!shouldActivate)
        {
            FirstPersonObject.SetActive(false);
            ThirdPersonObject.SetActive(false);
            return;
        }
        if (isOwner)
            FirstPersonObject.SetActive(true);
        else
            ThirdPersonObject.SetActive(false);
    }
}
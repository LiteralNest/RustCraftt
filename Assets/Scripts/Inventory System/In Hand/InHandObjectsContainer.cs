using System.Collections.Generic;
using UnityEngine;

public class InHandObjectsContainer : MonoBehaviour
{
    [SerializeField] private List<InHandObject> _inHandObjects;

    public void DisplayItems(int itemId)
    {
        foreach (var obj in _inHandObjects)
        {
            if (obj.TargetItem.Id == itemId)
            {
                obj.gameObject.SetActive(true);
                continue;
            }

            obj.gameObject.SetActive(false);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class InHandObjectsContainer : MonoBehaviour
{
    [SerializeField] private List<InHandObject> _inHandObjects;
    
    private void OnEnable()
        => GlobalEventsContainer.ShouldDisplayHandItem += DisplayItems;

    private void OnDisable()
        => GlobalEventsContainer.ShouldDisplayHandItem -= DisplayItems;

    private void DisplayItems(Item item)
    {
        foreach (var obj in _inHandObjects)
        {
            if (obj.TargetItem.Id == item.Id)
            {
                obj.gameObject.SetActive(true);
                continue;
            }

            obj.gameObject.SetActive(false);
        }
    }
}
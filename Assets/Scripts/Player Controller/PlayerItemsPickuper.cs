using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsPickuper : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private InventorySlotsContainer _inventorySlotsContainer;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;

    public void PickUp()
    {
        var item = _objectsRayCaster.LootingItem;
        if (item == null) return;
        _inventorySlotsContainer.AddItemToDesiredSlot(item.Item, item.Count);
        Destroy(item.gameObject);
    }
}

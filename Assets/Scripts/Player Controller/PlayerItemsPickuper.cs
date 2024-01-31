using Player_Controller;
using Storage_System;
using UnityEngine;

public class PlayerItemsPickuper : MonoBehaviour
{
    [Header("Attached Scripts")] [SerializeField]
    private Storage _inventorySlotsContainer;

    [SerializeField] private ObjectsRayCaster _objectsRayCaster;

    public void PickUp()
    {
        var item = _objectsRayCaster.LootingItem;
        if (item == null) return;
        _inventorySlotsContainer.AddItemToSlotWithAlert(item.Data.Value.Id, item.Data.Value.Count, item.Data.Value.Ammo,
                item.Data.Value.Hp);
        Destroy(item.gameObject);
    }
}
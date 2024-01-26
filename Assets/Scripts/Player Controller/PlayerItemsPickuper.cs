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
        _inventorySlotsContainer.AddItemToDesiredSlotServerRpc(item.Data.Value.Id, item.Data.Value.Count, item.Data.Value.Ammo,
                item.Data.Value.Hp);
        Destroy(item.gameObject);
    }
}
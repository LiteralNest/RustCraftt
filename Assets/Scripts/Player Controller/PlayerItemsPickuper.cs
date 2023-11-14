using UnityEngine;

public class PlayerItemsPickuper : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private Storage _inventorySlotsContainer;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;

    public void PickUp()
    {
        var item = _objectsRayCaster.LootingItem;
        if (item == null) return;
        _inventorySlotsContainer.AddItemToDesiredSlotServerRpc(item.ItemId.Value, item.Count.Value);
        Destroy(item.gameObject);
    }
}

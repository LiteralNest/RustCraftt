using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SlotDisplayer : MonoBehaviour, IDropHandler
{
    [field: SerializeField] public int Index { get; set; }
    [field: SerializeField] public InventoryItemDisplayer ItemDisplayer { get; protected set; }
    [field: SerializeField] public SlotsContainer Inventory { get; private set; }
    protected abstract void Drop(PointerEventData eventData);
    protected abstract void AddItem(InventoryItemDisplayer item);

    public void OnDrop(PointerEventData eventData)
        => Drop(eventData);

    public void Init(SlotsContainer slotsContainer)
        => Inventory = slotsContainer;

    public void Init(InventoryItemDisplayer itemDisplayer)
    {
        ItemDisplayer = itemDisplayer;
    }

    public abstract void ClearItem();

    protected bool TryAddToFreeSlot(InventoryItemDisplayer newItemDisplayer)
    {
        if (newItemDisplayer == null) return false;
        if (ItemDisplayer == null)
        {
            AddItem(newItemDisplayer);
            return true;
        }

        return false;
    }

    private void ClearPlace(Transform place)
    {
        for (int i = 0; i < place.childCount; i++)
        {
            var child = place.GetChild(i);
            if(!child.TryGetComponent(out InventoryItemDisplayer itemDisplayer)) continue;
            Destroy(child.gameObject);
            i--;
        }
    }
    
    public void DeleteItem()
    {
        ClearItem();
        ClearPlace(transform);
    }
}
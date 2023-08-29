using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDisplayer : MonoBehaviour, IDropHandler
{
    [field:SerializeField] public InventorySlotsContainer Inventory { get; private set; }
    [field:SerializeField] public int Index { get; set; }
    [field: SerializeField] public InventoryItemDisplayer ItemDisplayer { get; private set; }
    
    private void Start()
    {
        if (Inventory == null)
            Inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySlotsContainer>();
    }
    
    public void Init(InventoryItemDisplayer itemDisplayer)
        => ItemDisplayer = itemDisplayer;

    public void ClearItem()
    {
        Inventory.ResetItemAt(Index);
        ItemDisplayer = null;
    }
 
    public void AddItem(InventoryItemDisplayer item)
    {
        ItemDisplayer = item;
        item.Init(this);
        Inventory.SetItemAt(Index, item.InventoryCell);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemDisplayer newItemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        if (newItemDisplayer == null) return;
        if (ItemDisplayer == null)
        {
            AddItem(newItemDisplayer);
            return;
        }

        int togetherCount = ItemDisplayer.InventoryCell.Count + newItemDisplayer.InventoryCell.Count;
        if (ItemDisplayer.InventoryCell.Item.Id == newItemDisplayer.InventoryCell.Item.Id)
        {
            if (togetherCount >= ItemDisplayer.InventoryCell.Item.StackCount)
            {
                int diff = togetherCount - newItemDisplayer.InventoryCell.Item.StackCount;
                newItemDisplayer.SetCount(diff);
                ItemDisplayer.SetCount(togetherCount - diff);
                return;
            }

            ItemDisplayer.AddCount(newItemDisplayer.InventoryCell.Count);
            newItemDisplayer.MinusCount(newItemDisplayer.InventoryCell.Count);
        }
    }
}
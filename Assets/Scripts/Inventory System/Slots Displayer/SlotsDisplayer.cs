using System.Collections.Generic;
using Armor_System.UI;
using Inventory_System;
using Inventory_System.Inventory_Slot_Displayers;
using Storage_System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SlotsDisplayer : MonoBehaviour
{
   [field:SerializeField] public Storage TargetStorage { get; private set; }
    
    [FormerlySerializedAs("_cellDisplayers")] [Header("Start Init")] [SerializeField]
    public List<InventorySlotDisplayer> CellDisplayers = new List<InventorySlotDisplayer>();

    public abstract void InitItems();

    public virtual List<ArmorSlotDisplayer> GetArmorSlots()
        => null;
    private void Awake()
    {
        InitItems();
    }

    public void InitCells()
    {
        Debug.Log(gameObject.name);
        for (int i = 0; i < CellDisplayers.Count; i++)
            CellDisplayers[i].Init(i, this, TargetStorage);
    }

    public void ResetCells()
    {
        foreach (var cell in CellDisplayers)
            cell.DestroyItem();
    }

    private ItemDisplayer GetGeneratedItemDisplayer(InventoryCell cell, InventorySlotDisplayer slotDisplayer)
    {
        var itemDisplayer = Instantiate(InventorySlotDisplayerSelector.singleton.GetDisplayerByType(cell.Item),
            slotDisplayer.transform);
        itemDisplayer.Init(slotDisplayer, cell, TargetStorage);
        return itemDisplayer;
    }

    public virtual void DisplayCells()
    {
        ResetCells();
        var cells = TargetStorage.ItemsNetData.Value.Cells;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].Id == -1) continue;
            var item = ItemFinder.singleton.GetItemById(cells[i].Id);
            var inventoryCell = new InventoryCell(item, cells[i].Count, cells[i].Hp, cells[i].Ammo);
            CellDisplayers[i].DisplayItem(GetGeneratedItemDisplayer(inventoryCell, CellDisplayers[i]));
        }
    }
}
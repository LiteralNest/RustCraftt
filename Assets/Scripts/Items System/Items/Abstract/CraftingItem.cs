using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingItem")]
public class CraftingItem : Item
{
   [field: SerializeField] public List<CraftingItemDataTableSlot> NeededSlots;
   
   public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
   {
      base.Click(slotDisplayer, handler, out shouldMinus);
      shouldMinus = false;
   }
}

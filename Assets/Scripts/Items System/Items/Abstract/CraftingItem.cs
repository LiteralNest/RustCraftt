using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingItem")]
public class CraftingItem : Item
{
   [field: SerializeField] public List<CraftingItemDataTableSlot> NeededSlots;
   
   public override void Click(InventoryHandler handler)
   {
      throw new System.NotImplementedException();
   }
}

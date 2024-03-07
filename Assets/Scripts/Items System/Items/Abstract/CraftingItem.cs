using System.Collections.Generic;
using Crafting_System.Crafting_Item_Data_Displayer.CraftingItemDataTable;
using UnityEngine;

namespace Items_System.Items.Abstract
{
   [CreateAssetMenu(menuName = "Item/CraftingItem")]
   public class CraftingItem : Item
   {
      [field: SerializeField] public int TimeForCreating;
      [field: SerializeField] public List<CraftingItemDataTableSlot> NeededSlots;
      [field: SerializeField] public int NeededWorkBanch = 0;
      [field: SerializeField] public bool ResearchedByDefault;
   }
}

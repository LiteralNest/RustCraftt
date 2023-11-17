using System.Collections.Generic;
using UnityEngine;

public class CraftingItem : Item
{
   [field: SerializeField] public int TimeForCreating;
   [field: SerializeField] public List<CraftingItemDataTableSlot> NeededSlots;
   [field: SerializeField] public int NeededWorkBanch = 0;
}

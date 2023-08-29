using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingItem")]
public class CraftingItem : Item
{
   [field: SerializeField] public List<CraftingItemDataTableSlot> NeededSlots;
}

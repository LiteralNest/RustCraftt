using Items_System.Items.Abstract;
using UnityEngine;

[System.Serializable]
public struct CraftingItemDataTableSlot
{
    [field:SerializeField] public Item Resource { get; private set; }
    [field:SerializeField] public int Count { get; set; }

    public CraftingItemDataTableSlot(Item resource, int count)
    {
        Resource = resource;
        Count = count;
    }
}
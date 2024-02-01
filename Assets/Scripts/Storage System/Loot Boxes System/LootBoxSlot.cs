using Items_System.Items.Abstract;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [System.Serializable]
    public struct LootBoxSlot
    {
        public Item Item;
        [Range(0,100)]
        public int Chance;
        public Vector2Int RandCount;
    }
}
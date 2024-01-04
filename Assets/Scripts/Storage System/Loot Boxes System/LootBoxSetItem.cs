using Items_System.Items.Abstract;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [System.Serializable]
    public struct LootBoxSetItem
    {
        [field:SerializeField] public Item Item { get; private set; }
        [field:SerializeField] public int MinimalCount { get; private set; }
        [field: SerializeField] public int MaximalCount { get; private set; }
    }
}

using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Ore_Type
{
    [System.Serializable]
    public struct OreSlot
    {
        public Item Resource;
        public Vector2Int CountRange;
    }
}

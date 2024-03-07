using Items_System.Items.Abstract;
using UnityEngine;

namespace ResourceOresSystem
{
    [System.Serializable]
    public struct OreSlot
    {
        public Item Resource;
        public Vector2Int CountRange;
        public bool ShouldAddWithRand;
    }
}

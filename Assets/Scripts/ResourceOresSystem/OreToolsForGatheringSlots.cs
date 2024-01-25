using Items_System.Items;
using UnityEngine;

namespace ResourceOresSystem
{
    [System.Serializable]
    public struct OreToolsForGatheringSlots
    {
        public Tool Tool;
        [Range(0, 100)] public int LossAmount;
    }
}
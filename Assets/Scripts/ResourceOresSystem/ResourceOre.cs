using System.Collections.Generic;
using Items_System.Items;
using Items_System.Items.Abstract;
using ResourceOresSystem;
using UnityEngine;

namespace Items_System.Ore_Type
{
    public class ResourceOre : Ore
    {
        [SerializeField]
        private List<OreToolsForGatheringSlots> _toolsForGathering = new List<OreToolsForGatheringSlots>();

        [field: SerializeField] public AudioClip GatheringClip { get; private set; }


        protected void Start()
        {
            gameObject.tag = "Ore";
        }

        private bool SlotFound(Item item, out OreToolsForGatheringSlots slot)
        {
            foreach (var tool in _toolsForGathering)
            {
                if (tool.Tool.Id != item.Id) continue;
                slot = tool;
                return true;
            }

            slot = default;
            return false;
        }
        
        public override bool CanUseTool(Tool tool)
            => SlotFound(tool, out OreToolsForGatheringSlots toolSlot);

        public void MinusHp(Item targetTool, out bool destroyed, Vector3 lastRayPos, Vector3 lastRayRot)
        {
            destroyed = false;
            if (_currentHp.Value <= 0) return;
            if (!SlotFound(targetTool, out OreToolsForGatheringSlots toolSlot)) return;
            AddResourcesToInventory(targetTool as Tool, toolSlot);
            DisplayVfxServerRpc(lastRayPos, lastRayRot);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentHp(1);
            MinusHpServerRpc();
            destroyed = _currentHp.Value <= 0;
        }
    }
}
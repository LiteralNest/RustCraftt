using System.Collections.Generic;
using Items_System.Items.Abstract;
using ResourceOresSystem;
using UnityEngine;

namespace Items_System.Ore_Type
{
    public class ResourceOre : Ore
    {
        [SerializeField] private List<Item> _toolsForGathering = new List<Item>();
    
        [field: SerializeField] public AudioClip GatheringClip { get; private set; }

    
        protected override void Start()
        {
            base.Start();
            gameObject.tag = "Ore";
        }
        public bool CanUseTool(Item tool)
            => _toolsForGathering.Contains(tool);

        public void MinusHp(Item targetTool, out bool destroyed, Vector3 lastRayPos, Vector3 lastRayRot)
        {
            destroyed = false;
            if (_currentHp.Value <= 0) return;
            if(!CanUseTool(targetTool)) return;
            AddResourcesToInventory();
            DisplayVfxServerRpc(lastRayPos, lastRayRot);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentHp(1);
            MinusHpServerRpc();
            destroyed = _currentHp.Value <= 0;
        }
    }
}
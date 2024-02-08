using System.Collections.Generic;
using Inventory_System;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Blocks
{
    public class BuildingStructure : NetworkBehaviour
    {
        [field: SerializeField] public NetworkObject NetObject { get; private set; }
        [field: SerializeField] public int Id { get; private set; }
        [SerializeField] private List<BuildingBlock> _blocks = new List<BuildingBlock>();

        private void Awake()
        {
            if(NetObject == null)
                NetObject = GetComponent<NetworkObject>();
        }

        public List<InventoryCell> GetPlacingRemovingCells()
        {
            List<InventoryCell> res = new List<InventoryCell>();
            foreach(var block in _blocks)
                res.AddRange(block.GetNeededCellsForPlacing());
            return res;
        }
    }
}
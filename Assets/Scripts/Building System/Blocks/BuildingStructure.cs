using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Blocks
{
    public class BuildingStructure : NetworkBehaviour
    {
        [field: SerializeField] public NetworkObject NetObject { get; private set; }
        [field: SerializeField] public int Id { get; private set; }

        public List<BuildingBlock> Blocks => _blocks;
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
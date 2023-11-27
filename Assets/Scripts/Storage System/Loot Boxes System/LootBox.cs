using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootBox : Storage
    {
        [SerializeField] private List<LootBoxGeneratingSet> _setsPool = new List<LootBoxGeneratingSet>();

        private void Start()
        {
            GenerateCells();
        }
        

        public override void Open(InventoryHandler handler)
        { 
            handler.InventoryPanelsDisplayer.OpenLootBoxPanel();
            SlotsDisplayer = handler.LootBoxSlotsDisplayer;
            base.Open(handler);
        }
    
        public override void CheckCells()
        {
            CheckCellsServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void CheckCellsServerRpc()
        {
            if(!IsServer) return;
            var cells = ItemsNetData.Value.Cells;
            foreach (var cell in cells)
                if (cell.Id != -1)
                    return;
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    
        private LootBoxGeneratingSet GetRandomSet()
            => _setsPool[Random.Range(0, _setsPool.Count)];
    
        private void GenerateCells()
        {
            var set = GetRandomSet();
            for (int i = 0; i < set.Items.Count; i++)
            {
                SetItemServerRpc(i, new CustomSendingInventoryDataCell(set.Items[i].Item.Id, Random.Range(set.Items[i].MinimalCount, set.Items[i].MaximalCount + 1), 100));
            }
        }
    }
}
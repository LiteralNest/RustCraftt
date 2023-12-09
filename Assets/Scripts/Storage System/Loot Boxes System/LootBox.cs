using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootBox : Storage
    {
        [SerializeField] private List<LootBoxGeneratingSet> _setsPool = new List<LootBoxGeneratingSet>();
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            GenerateCells();
        }

        protected override void DoAfterResetItem()
        {
            base.DoAfterResetItem();
            CheckCellsServerRpc();
        }
        
        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer = handler.LootBoxSlotsDisplayer;
            base.Open(handler);
            handler.InventoryPanelsDisplayer.OpenLootBoxPanel();
        }

        [ServerRpc(RequireOwnership = false)]
        private void CheckCellsServerRpc()
        {
            if(!IsServer) return;
            var cells = ItemsNetData.Value.Cells;
            foreach (var cell in cells)
                if (cell.Id != -1)
                    return;
            InventoryHandler.singleton.InventoryPanelsDisplayer.OpenLootBoxPanel(false);
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
                AddItemToDesiredSlotServerRpc(set.Items[i].Item.Id, Random.Range(set.Items[i].MinimalCount, set.Items[i].MaximalCount + 1), 0);
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
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
            if(!IsServer) return;
            Debug.Log("Generating cells...");
            GenerateCells();
        }

        protected override void DoAfterResetItem()
        {
            base.DoAfterResetItem();
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
        
        [ContextMenu("Generate Cells")]
        private async void GenerateCells()
        {
            await Task.Delay(2000); //Server Waits 2 seconds
            var set = GetRandomSet();
            for (int i = 0; i < set.Items.Count; i++)
            {
                var rand = Random.Range(set.Items[i].MinimalCount, set.Items[i].MaximalCount + 1);
                AddItemToDesiredSlotServerRpc(set.Items[i].Item.Id, rand, 0);
                Debug.Log("Item id: " + set.Items[i].Item.Id + "; Count: " + rand);
            }
            Debug.Log("Cells generated!");
        }
    }
}
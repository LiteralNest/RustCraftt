using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootBox : Storage
    {
        [SerializeField] private string _setsPath = "Loot Box Generating Sets";

        private List<LootBoxGeneratingSet> _setsPool = new List<LootBoxGeneratingSet>();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsServer) return;
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
            if (!IsServer) return;
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
        private void GenerateCells()
        {
            _setsPool = Resources.LoadAll<LootBoxGeneratingSet>(_setsPath).ToList();
            var set = GetRandomSet();
            for (int i = 0; i < set.Items.Count; i++)
            {
                var rand = Random.Range(set.Items[i].MinimalCount, set.Items[i].MaximalCount + 1);
                AddItemToDesiredSlot(set.Items[i].Item.Id, rand, 0);
                Debug.Log("Item id: " + set.Items[i].Item.Id + "; Count: " + rand);
            }

            Debug.Log("Cells generated!");
        }
    }
}
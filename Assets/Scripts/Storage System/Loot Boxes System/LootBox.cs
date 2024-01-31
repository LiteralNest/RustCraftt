using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootBox : Storage
    {
        [SerializeField] private LootBoxSlot _scrap;
        [SerializeField]  private List<LootBoxSlot> _setsPool = new List<LootBoxSlot>();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsServer) return;
            GenerateCells();
            ItemsNetData.OnValueChanged += (oldValue, newValue) => CheckCells();
        }
        
        private void CheckCells()
        {
            var cells = ItemsNetData.Value.Cells;
            foreach (var cell in cells)
                if (cell.Id != -1)
                    return;
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }

        [ContextMenu("Generate Cells")]
        private void GenerateCells()
        {
            AddItemToDesiredSlot(_scrap.Item.Id, Random.Range(_scrap.RandCount.x, _scrap.RandCount.y + 1), 0);
            foreach (var set in _setsPool)
            {
                var rand = Random.Range(0, 100);
                if (rand > set.Chance) continue;
                AddItemToDesiredSlot(set.Item.Id, Random.Range(set.RandCount.x, set.RandCount.y + 1), 0);
                return;
            }   
        }
    }
}
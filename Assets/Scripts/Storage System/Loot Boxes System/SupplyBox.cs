using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    public class SupplyBox : LootBox
    {
        [Header("Supply Box")] [SerializeField]
        private Vector2 _randomCountRange = new Vector2(2, 6);

        protected override void GenerateCells()
        {
            var cachedSet = new List<LootBoxSlot>(_setsPool);
            var randomCount = Random.Range(_randomCountRange.x, _randomCountRange.y + 1);
            for (int i = 0; i < randomCount; i++)
            {
                var set = GetRandomSlot(cachedSet);
                cachedSet.Remove(set);
                AddItemToDesiredSlot(set.Item.Id, Random.Range(set.RandCount.x, set.RandCount.y + 1), 0);
            }
        }
        
        protected override void CheckCells()
        {
            if (!StorageEmpty()) return;
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
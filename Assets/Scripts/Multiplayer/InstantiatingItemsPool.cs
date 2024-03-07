using System.Collections.Generic;
using System.Linq;
using Items_System;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public class InstantiatingItemsPool : NetworkBehaviour
    {
        private const string Path = "Looting Items";
        
        public static InstantiatingItemsPool sigleton { get; set; }

        [SerializeField] private List<LootingItem> _items = new List<LootingItem>();

        [SerializeField] private LootingItem _universalDropableItem;

        private void Awake()
        {
            sigleton = this;
            _items = Resources.LoadAll<LootingItem>(Path).ToList();
        }
       
        private void SpawnLoot(LootingItem lootingItem, Vector3 position)
        {
            lootingItem.NetworkObject.DontDestroyWithOwner = true;
            lootingItem.NetworkObject.Spawn();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnObjectServerRpc(CustomSendingInventoryDataCell data, Vector3 position)
        {
            if (!IsServer) return;
            SpawnObjectOnServer(data, position);
        }

        public void SpawnObjectOnServer(CustomSendingInventoryDataCell data, Vector3 position)
        {
         
            LootingItem lootingItem = null;
            foreach (var item in _items)
            {
                if (item.TargetItem.Id != data.Id) continue;
                lootingItem = Instantiate(item, position, Quaternion.identity);
                SpawnLoot(lootingItem, position);
                lootingItem.transform.position = position;
                lootingItem.Init(data);
                return;
            }
            lootingItem = Instantiate(_universalDropableItem, position, Quaternion.identity);
            SpawnLoot(lootingItem, position);
            lootingItem.Init(data);
        }
    }
}
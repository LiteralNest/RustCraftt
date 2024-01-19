using Items_System.Items.Abstract;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Items_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootingItem : NetworkBehaviour
    {
        public CustomSendingInventoryDataCell Data { get; set; }
       [field:SerializeField] public Item TargetItem { get; private set; }

        private void Start()
            => gameObject.tag = "LootingItem";

        [ServerRpc(RequireOwnership = false)]
        public void PickUpServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }

            Destroy(gameObject);
        }
    }
}
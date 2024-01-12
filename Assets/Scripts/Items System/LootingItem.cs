using Unity.Netcode;
using UnityEngine;

namespace Items_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootingItem : NetworkBehaviour
    {
        public NetworkVariable<int> ItemId = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> Count = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

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
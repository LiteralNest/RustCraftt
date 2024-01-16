using Unity.Netcode;
using UnityEngine;

namespace Items_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootingItem : NetworkBehaviour
    {
        public int ItemId
        {
            get => _itemId.Value;
            set => _itemId.Value = value;
        }

        public int Count
        {
            get => _count.Value;
            set => _count.Value = value;
        }

        [SerializeField] private NetworkVariable<int> _itemId = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private NetworkVariable<int> _count = new(0, NetworkVariableReadPermission.Everyone,
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
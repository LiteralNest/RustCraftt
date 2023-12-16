using System.Linq;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public class BackPack : Storage
    {
        [SerializeField] private NetworkObject _networkObject;
        public NetworkVariable<bool> WasDisconnected { get; private set; } = new(false);
        public NetworkVariable<int> OwnerId { get; private set; } = new(-1);

        [ServerRpc(RequireOwnership = false)]
        public void SetWasDisconnectedAndOwnerIdServerRpc(bool value, int ownerId)
        {
            if (!IsServer) return;
            WasDisconnected.Value = value;
            OwnerId.Value = ownerId;
        }

        [ServerRpc(RequireOwnership = false)]
        public void AssignCorpServerRpc(int id)
        {
            if (!IsServer) return;
            AssignCorpClientRpc(id);
        }

        [ClientRpc]
        private void AssignCorpClientRpc(int id)
        {
            var copres = FindObjectsOfType<PlayerCorpes>().ToList();
            foreach (var corp in copres)
            {
                if (corp.Id != id) continue;
                corp.transform.SetParent(transform);
                corp.transform.localPosition = Vector3.zero;
                corp.transform.localRotation = Quaternion.identity;
                Destroy(corp);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void DespawnServerRpc()
        {
            if (!IsServer) return;
            _networkObject.Despawn();
        }
    }
}
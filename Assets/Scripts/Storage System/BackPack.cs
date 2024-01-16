using System.Linq;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public class BackPack : Storage
    {
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Transform _parrentingObject;
        [SerializeField] private Transform _mapPoint;
        public NetworkVariable<bool> WasDisconnected { get; private set; } = new(false);
        public NetworkVariable<int> OwnerId { get; private set; } = new(-1);

        private void OnDestroy()
            => Destroy(_mapPoint.gameObject);

        [ServerRpc(RequireOwnership = false)]
        public void SetWasDisconnectedAndOwnerIdServerRpc(bool value, int ownerId)
        {
            if (!IsServer) return;
            WasDisconnected.Value = value;
            OwnerId.Value = ownerId;
        }

        [ClientRpc]
        public void AssignCorpClientRpc(int id)
        {
            var copres = FindObjectsOfType<PlayerCorpes>().ToList();
            foreach (var corp in copres)
            {
                if (corp.Id != id) continue;
                corp.transform.SetParent(_parrentingObject);
                corp.transform.localPosition = Vector3.zero;
                corp.transform.localRotation = Quaternion.identity;
                _mapPoint.gameObject.SetActive(true);
                _mapPoint.SetParent(null);
                _mapPoint.eulerAngles = new Vector3(-90, 0, 90);
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
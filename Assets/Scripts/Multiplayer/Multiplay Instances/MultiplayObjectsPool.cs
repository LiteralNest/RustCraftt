using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer.Multiplay_Instances
{
    public class MultiplayObjectsPool : NetworkBehaviour
    {
        public static MultiplayObjectsPool singleton { get; private set; }

        private void Awake()
            => singleton = this;

        [SerializeField] private List<MultiplayInstanceId> _multiplayInstanceIds = new List<MultiplayInstanceId>();

        [ServerRpc(RequireOwnership = false)]
        public void InstantiateObjectServerRpc(int id, Vector3 pos, Quaternion rot, float throwForce, Vector3 throwDirection)
        {
            if (!IsServer) return;
            var obj = Instantiate(GetMultiplayInstanceIdById(id), pos, rot);
            obj.GetComponent<NetworkObject>().Spawn(true);
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }

        private MultiplayInstanceId GetMultiplayInstanceIdById(int id)
            => _multiplayInstanceIds.Find(x => x.Id == id);
    }
}
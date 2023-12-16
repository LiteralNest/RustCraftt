using System.Linq;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public class BackPack : Storage
    {
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
    }
}
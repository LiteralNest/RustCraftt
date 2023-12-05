using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerMainComponentsRemover : NetworkBehaviour
    {
        [SerializeField] private List<Behaviour> _removingComponents;
        [SerializeField] private List<GameObject> _removingObjects;

        [ServerRpc(RequireOwnership = false)]
        public void RemoveMainComponentsServerRpc()
        {
            if(!IsServer) return;
            RemoveMainComponentsClientRpc();
        }

        [ClientRpc]
        private void RemoveMainComponentsClientRpc()
        {
            foreach (var component in _removingComponents)
            {
                if (component == null) continue;
                Destroy(component);
            }
            
            foreach (var obj in _removingObjects)
            {
                if (obj == null) continue;
                Destroy(obj);
            }
        }

        [ContextMenu("RemoveComponents")]
        private void RemoveComponents()
        {
            RemoveMainComponentsServerRpc();
        }
    }
}

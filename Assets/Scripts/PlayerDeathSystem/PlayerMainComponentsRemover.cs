using System.Collections.Generic;
using Storage_System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerDeathSystem
{
    public class PlayerMainComponentsRemover : NetworkBehaviour
    {
        [SerializeField] private CharacterInventory _characterInventory;
        [SerializeField] private List<Behaviour> _removingComponents;
        [SerializeField] private List<GameObject> _removingObjects;
        [SerializeField] private Transform _characterView;

        [FormerlySerializedAs("_playerCorpesHanlder")] [SerializeField] private PlayerCorpesHanler playerCorpesHanler;

        [ServerRpc(RequireOwnership = false)]
        public void RemoveMainComponentsServerRpc()
        {
            if(!IsServer) return;
            RemoveMainComponentsClientRpc();
        }

        [ClientRpc]
        private void RemoveMainComponentsClientRpc()
        {
            playerCorpesHanler.AssignCorpes(_characterInventory.ItemsNetData.Value);
            
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

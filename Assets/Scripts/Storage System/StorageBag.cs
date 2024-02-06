using System.Collections;
using InteractSystem;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public class StorageBag : NetworkBehaviour, IRaycastInteractable
    {
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Storage _targetStorage;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(IsServer)
                StartCoroutine(DespawnRoutine());
        }
        
        public string GetDisplayText()
            => "Open";

        public void Interact()
            => _targetStorage.Interact();

        public bool CanInteract()
            => true;

        private IEnumerator DespawnRoutine()
        {
            yield return new WaitForSeconds(1200f);
            _networkObject.Despawn();
        }
    }
}
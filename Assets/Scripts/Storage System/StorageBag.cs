using InteractSystem;
using UnityEngine;

namespace Storage_System
{
    public class StorageBag : MonoBehaviour, IRaycastInteractable
    {
        [SerializeField] private Storage _targetStorage;

        public string GetDisplayText()
            => "Open";

        public void Interact()
            => _targetStorage.Interact();

        public bool CanInteract()
            => true;
    }
}
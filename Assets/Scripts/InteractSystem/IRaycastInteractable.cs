using UnityEngine;

namespace InteractSystem
{
    public interface IRaycastInteractable
    {
        public string GetDisplayText();
        public void Interact();
        public Sprite GetIcon();
        public bool CanInteract();
        public bool CanDisplayInteract();
    }
}
namespace InteractSystem
{
    public interface IRaycastInteractable
    {
        public string GetDisplayText();
        public void Interact();
        public bool CanInteract();
    }
}
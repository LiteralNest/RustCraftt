namespace Building_System.Upgrading
{
    public interface IHammerInteractable
    {
        public InventoryCell GetNeededItemsForUpgrade();
        public bool CanBeUpgraded();
        public void Upgrade();
    
        public bool CanBeRepaired();
        public void Repair();
    
        public bool CanBeDestroyed();
        public void Destroy();

        public bool CanBePickUp();
        public void PickUp();
    }
}
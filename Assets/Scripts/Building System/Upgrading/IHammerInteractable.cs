using System.Collections.Generic;

namespace Building_System.Upgrading
{
    public interface IHammerInteractable
    {
        public bool CanBeUpgraded();
        public List<InventoryCell> GetNeededCellsForUpgrade();
        public void UpgradeTo(int level);
        public int GetLevel();
    
        public bool CanBeRepaired();
        public void Repair();
    
        public bool CanBeDestroyed();
        public void Destroy();

        public bool CanBePickUp();
        public void PickUp();
    }
}
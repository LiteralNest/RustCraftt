using System.Collections.Generic;
using Inventory_System;

namespace Building_System.Upgrading
{
    public interface IHammerInteractable
    {
        public bool CanBeUpgraded(int level);
        public List<InventoryCell> GetNeededCellsForUpgrade(int level);
        public void UpgradeTo(int level);

        public bool CanBeRepaired();
        public void Repair();

        public bool CanBeDestroyed();
        public void Destroy();

        public bool CanBePickUp();
        public void PickUp();
    }
}
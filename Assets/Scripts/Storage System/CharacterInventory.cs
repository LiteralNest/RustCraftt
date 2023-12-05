using Building_System.NetWorking;

namespace Storage_System
{
    public class CharacterInventory : Storage
    {

        public override void OnNetworkDespawn()
        {
            
            PlayerStaffSpawner.Singleton.SpawnSleepServerRpc(transform.position, transform.rotation);
        }

        
        protected override void Appear()
        {
        }

        protected override void DoAfterRemovingItem(InventoryCell cell)
        {
            GlobalEventsContainer.OnInventoryItemRemoved?.Invoke(cell);
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        protected override void DoAfterAddingItem(InventoryCell cell)
        {
            GlobalEventsContainer.OnInventoryItemAdded?.Invoke(cell);
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer = handler.InventorySlotsDisplayer;
            base.Open(handler);
        }
    }
}
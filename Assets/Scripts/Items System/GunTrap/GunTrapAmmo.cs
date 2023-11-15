using Storage_Boxes;

namespace Items_System.GunTrap
{
    public class GunTrapAmmo : Storage
    {
        public bool CanShot()
            => GetCellWithAmmo() != null;

        private InventoryCell GetCellWithAmmo()
        {
            foreach(var cell in Cells)
                if(cell.Item != null && cell.Count > 0)
                    return cell;
            return null;
            
        }
        
        private void CheckAmmo(InventoryCell cell)
        {
            if(cell.Count <= 0)
                cell.Item = null;
        }

        public void RemoveAmmo()
        {
            var cell = GetCellWithAmmo();
            cell.Count--;
            CheckAmmo(cell);
        }

        public override void Open(InventoryHandler handler)
        {
            base.Open(handler);
            handler.OpenShotGunPanel();
            SlotsDisplayer = handler.ShotGunSlotsDisplayer;
        }
    }
}
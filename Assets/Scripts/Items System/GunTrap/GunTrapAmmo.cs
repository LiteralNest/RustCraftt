using Storage_System;

namespace Items_System.GunTrap
{
    public class GunTrapAmmo : Storage
    {
        public bool CanShot()
            => GetCellWithAmmo().Id != -1;

        private CustomSendingInventoryDataCell GetCellWithAmmo()
        {
            foreach (var cell in ItemsNetData.Value.Cells)
                if (cell.Id != -1 && cell.Count > 0)
                    return cell;
            return new CustomSendingInventoryDataCell(-1, 0, -1);
        }

        private int GetCellWithAmmoId()
        {
            for (int i = 0; i < ItemsNetData.Value.Cells.Length; i++)
            {
                if (ItemsNetData.Value.Cells[i].Id != -1 && ItemsNetData.Value.Cells[i].Count > 0)
                    return i;
            }
            return -1;
        }

        public void RemoveAmmo()
        {
            var cellId = GetCellWithAmmoId();
            RemoveItemCountServerRpc(cellId, 1);
        }

        public override void Open(InventoryHandler handler)
        {
            base.Open(handler);
            handler.InventoryPanelsDisplayer.OpenShotGunPanel();
            SlotsDisplayer = handler.ShotGunSlotsDisplayer;
        }
    }
}
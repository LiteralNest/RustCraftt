using Building_System.Blocks;
using Building_System.Blue_Prints;
using Building_System.NetWorking;

namespace Building_System.Placing_Objects
{
    public class PlacingObjectBluePrint : BluePrint
    {
        public PlacingObject TargetPlacingObject;

        protected virtual bool CanBePlaced()
        {
            foreach (var cell in BluePrintCells)
                if (!cell.CanBePlaced)
                    return false;
            return true;
        }

        public override void Place()
        {
            if (!CanBePlaced()) return;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(TargetPlacingObject.TargetItem.Id, 1);
            PlacingObjectsPool.singleton.InstantiateObjectServerRpc(TargetPlacingObject.TargetItem.Id,
                transform.position,
                transform.rotation);
        }

        public override void InitPlacedObject(BuildingStructure structure){}
    }
}
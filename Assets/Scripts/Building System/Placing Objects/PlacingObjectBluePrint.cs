using UnityEngine;

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
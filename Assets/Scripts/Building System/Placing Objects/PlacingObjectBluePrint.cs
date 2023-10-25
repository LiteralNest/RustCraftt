using UnityEngine;

public class PlacingObjectBluePrint : BluePrint
{
    public PlacingObject TargetPlacingObject;
    private bool _rotatedSide;

    private bool CanBePlaced()
    {
        foreach (var cell in BluePrintCells)
            if (!cell.CanBePlaced)
                return false;
        return true;
    }

    public override void Place()
    {
        if (!CanBePlaced()) return;
        InventorySlotsContainer.singleton.RemoveItemFromDesiredSlot(TargetPlacingObject.TargetItem, 1);
        PlacingObjectsPool.singleton.InstantiateObjectServerRpc(TargetPlacingObject.TargetItem.Id,
            transform.position,
            transform.rotation);
    }

    public override BuildingStructure GetBuildingStructure()
        => TargetPlacingObject;

    public void Rotate()
    {
        transform.eulerAngles += new Vector3(0, 90, 0);
        _rotatedSide = !_rotatedSide;
    }
}
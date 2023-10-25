using UnityEngine;

public class BuildingBluePrint : BluePrint
{
    public BuildingStructure TargetBuildingStructure;
    public override void Place()
    {
        foreach (var cell in BluePrintCells)
            cell.TryPlace();
    }

    public override BuildingStructure GetBuildingStructure()
        => TargetBuildingStructure;
}
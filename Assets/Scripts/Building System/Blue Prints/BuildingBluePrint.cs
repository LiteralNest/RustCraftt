public class BuildingBluePrint : BluePrint
{
    public override void Place()
    {
        foreach (var cell in BluePrintCells)
            cell.TryPlace();
    }
}
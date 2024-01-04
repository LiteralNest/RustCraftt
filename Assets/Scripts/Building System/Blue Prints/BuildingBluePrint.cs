using Building_System.Blocks;

namespace Building_System.Blue_Prints
{
    public class BuildingBluePrint : BluePrint
    {
        public override void Place()
        {
            if(!EnoughMaterials()) return;
            foreach (var cell in BluePrintCells)
                cell.TryPlace();
        }

        public override void InitPlacedObject(BuildingStructure structure)
        {
        
        }
    }
}
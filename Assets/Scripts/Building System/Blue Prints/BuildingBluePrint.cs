using Building_System.Building.Blocks;

namespace Building_System.Blue_Prints
{
    public class BuildingBluePrint : BluePrint
    {
        public override void Place()
        {
            if (!EnoughMaterials()) return;

            bool playedSound = false;

            foreach (var cell in BluePrintCells)
            {
                cell.TryPlace(!playedSound);
                playedSound = true;
            }
        }
        
        public override void InitPlacedObject(BuildingStructure structure)
        {
        }
    }
}
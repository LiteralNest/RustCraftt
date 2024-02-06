namespace Building_System.Blue_Prints
{
    public class KeyBluePrintCell : BuildingBluePrintCell
    {
        public override bool CanBePlace()
            => !OnFrontOfPlayer;
    } 
}

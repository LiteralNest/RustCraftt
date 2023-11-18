using UnityEngine;

public class ClipBoardBluePrintCell : BuildingBluePrintCell
{
    [SerializeField] private ClipBoardTrigger _clipBoardTrigger; 
    public override bool CanBePlace()
    {
        if (_clipBoardTrigger.IsInsideOtherClipBoard) return false;
        return base.CanBePlace();
    }

   
}

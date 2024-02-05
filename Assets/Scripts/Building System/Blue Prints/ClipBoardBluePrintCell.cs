using Building_System.Building.Placing_Objects.ClipBoard;
using UnityEngine;

namespace Building_System.Blue_Prints
{
    public class ClipBoardBluePrintCell : BuildingBluePrintCell
    {
        [SerializeField] private ClipBoardTrigger _clipBoardTrigger; 
        public override bool CanBePlace()
        {
            if (_clipBoardTrigger.IsInsideOtherClipBoard) return false;
            return base.CanBePlace();
        }

   
    }
}

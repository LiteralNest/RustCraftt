using UnityEngine;

namespace Building_System.Building.Placing_Objects.ClipBoard
{
    public class ClipBoardTrigger : MonoBehaviour
    {
        public bool IsInsideOtherClipBoard { get; private set; }
    
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("ShelfZone")) return;
            IsInsideOtherClipBoard = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("ShelfZone")) return;
            IsInsideOtherClipBoard = false;
        }
    }
}

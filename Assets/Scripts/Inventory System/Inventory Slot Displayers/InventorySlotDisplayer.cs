using Player_Controller;
using UnityEngine.EventSystems;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public class InventorySlotDisplayer : SlotDisplayer, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            PlayerNetCode.Singleton.ItemInfoHandler.ResetPanel();
        }
    }
}
using Building_System.Placing_Objects;
using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items.Abstract;
using UI;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Building")]
    public class Building : CraftingItem
    {
        [SerializeField] private PlacingObjectBluePrint _targetBluePrint;
    
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            InventoryHandler.singleton.PlayerObjectsPlacer.SetCurrentPref(_targetBluePrint);
            CharacterUIHandler.singleton.ActivatePlacingPanel(true);
        }
    }
}
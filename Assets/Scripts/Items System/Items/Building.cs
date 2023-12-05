using Building_System.Placing_Objects;
using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Building")]
    public class Building : CraftingItem
    {
        [SerializeField] private PlacingObjectBluePrint _targetBluePrint;
    
        public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
        {
            base.Click(slotDisplayer,handler);
            handler.PlayerObjectsPlacer.SetCurrentPref(_targetBluePrint);
            CharacterUIHandler.singleton.ActivatePlacingPanel(true);
        }
    }
}
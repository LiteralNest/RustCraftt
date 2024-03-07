using Building_System.Building.Placing_Objects;
using Events;
using Inventory_System;
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
            InventoryHandler.singleton.PlayerObjectsDragger.SetCurrentPref(_targetBluePrint);
            CharacterUIHandler.singleton.ActivatePlacingPanel(true);
        }

        public override void OnClickDisabled()
        {
            GlobalEventsContainer.BluePrintDeactivated?.Invoke();
        }
    }
}
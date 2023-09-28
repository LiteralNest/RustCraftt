using UnityEngine;

[CreateAssetMenu(menuName = "Item/Building")]
public class Building : CraftingItem
{
    [SerializeField] private PlacingObjectBluePrint _targetBluePrint;
    
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        shouldMinus = false;
        handler.PlayerObjectsPlacer.SetCurrentPref(_targetBluePrint);
        GlobalEventsContainer.ShouldDisplayPlacingPanel?.Invoke(true);
    }
}
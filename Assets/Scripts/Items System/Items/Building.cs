using UnityEngine;

[CreateAssetMenu(menuName = "Item/Building")]
public class Building : CraftingItem
{
    [SerializeField] private PlacingObjectBluePrint _targetBluePrint;
    
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
    {
        base.Click(slotDisplayer,handler);
        handler.PlayerObjectsPlacer.SetCurrentPref(_targetBluePrint);
        MainUiHandler.singleton.ActivatePlacingPanel(true);
    }
}
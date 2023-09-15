using UnityEngine;

[CreateAssetMenu(menuName = "Item/Building")]
public class Building : CraftingItem
{
    [SerializeField] private ObjectBluePrint _targetBluePrint;
    
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        shouldMinus = true;
        handler.BuildingChooser.ChooseBuilding(_targetBluePrint);
    }
}
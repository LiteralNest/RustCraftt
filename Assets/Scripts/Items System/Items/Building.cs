using UnityEngine;

[CreateAssetMenu(menuName = "Item/Building")]
public class Building : CraftingItem
{
   [field:SerializeField] public ObjectBluePrint TargetObject { get; private set; }
   
   public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
   {
      base.Click(slotDisplayer, handler, out shouldMinus);
      handler.SetActiveItem(this);
      shouldMinus = false;
      handler.ObjectPlacer.SetObject(TargetObject);
   }
}

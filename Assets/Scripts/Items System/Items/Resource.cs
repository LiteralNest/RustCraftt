using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource")]
public class Resource : Item
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        shouldMinus = false;
    }
}
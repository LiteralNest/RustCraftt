using UnityEngine;

[CreateAssetMenu(menuName = "Item/Ammo")]
public class Ammo : CraftingItem
{
    public float MultiplyKoef => _multiplyKoef;
    [Range(1, 5)]
    [Header("Ammo")] [SerializeField] private float _multiplyKoef = 1;

    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        shouldMinus = false;
    }
}
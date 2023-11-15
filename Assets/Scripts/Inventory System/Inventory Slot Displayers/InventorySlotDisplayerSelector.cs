using Inventory_System.Inventory_Items_Displayer;
using Inventory_System.Inventory_Slot_Displayers;
using UnityEngine;

public class InventorySlotDisplayerSelector : MonoBehaviour
{
    public static InventorySlotDisplayerSelector singleton;

    [SerializeField] private InventoryItemDisplayer _inventoryItemDisplayer;
    [SerializeField] private DamagableItemDisplayer _damagableItemDisplayer;
    [SerializeField] private LongRangeWeaponItemDisplayer _longRangeWeaponItemDisplayer;

    private void Awake()
        => singleton = this;

    public InventoryItemDisplayer GetDisplayerByType(Item item)
    {
        if (item is ShootingWeapon) return _longRangeWeaponItemDisplayer;
        if (item is Tool || item is MeleeWeapon) return _damagableItemDisplayer;
        return _inventoryItemDisplayer;
    }
}
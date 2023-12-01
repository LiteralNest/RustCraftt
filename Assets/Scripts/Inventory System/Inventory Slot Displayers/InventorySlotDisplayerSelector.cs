using Inventory_System.Inventory_Items_Displayer;
using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items;
using Items_System.Items.Abstract;
using Items_System.Items.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

public class InventorySlotDisplayerSelector : MonoBehaviour
{
    public static InventorySlotDisplayerSelector singleton;

    [SerializeField] private InventoryItemDisplayer _inventoryItemDisplayer;
    [SerializeField] private ToolItemDisplayer _toolItemDisplayer;
    [SerializeField] private LongRangeWeaponItemDisplayer _longRangeWeaponItemDisplayer;

    private void Awake()
        => singleton = this;

    public InventoryItemDisplayer GetDisplayerByType(Item item)
    {
        if (item is ShootingWeapon) return _longRangeWeaponItemDisplayer;
        if (item is Tool || item is MeleeWeapon || item is Armor) return _toolItemDisplayer;
        return _inventoryItemDisplayer;
    }
}
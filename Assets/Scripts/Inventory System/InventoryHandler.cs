using System.Collections;
using System.Threading.Tasks;
using ArmorSystem.Backend;
using Character_Stats;
using Fight_System.Weapon.ShootWeapon;
using Inventory_System;
using Inventory_System.Inventory_Items_Displayer;
using Items_System.Items.Abstract;
using Player_Controller;
using Storage_System;
using Unity.Netcode;
using UnityEngine;
using Vehicle;

public class InventoryHandler : NetworkBehaviour
{
    public static InventoryHandler singleton { get; set; }

    [field: SerializeField] public InventoryPanelsDisplayer InventoryPanelsDisplayer { get; private set; }
    [field: SerializeField] public PlayerNetCode PlayerNetCode { get; private set; }
    [field: SerializeField] public CharacterStats Stats { get; private set; }
    [field: SerializeField] public PlayerObjectsPlacer PlayerObjectsPlacer { get; private set; }
    [field: SerializeField] public InHandObjectsContainer InHandObjectsContainer { get; private set; }
    [field: SerializeField] public ArmorsContainer ArmorsContainer { get; private set; }
    [field: SerializeField] public SlotsDisplayer ShotGunSlotsDisplayer { get; private set; }
    [field: SerializeField] public InventorySlotsDisplayer InventorySlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer ToolClipboardSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer LargeStorageSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer LootBoxSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer CampFireSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer FurnaceSlotsDiaplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer RecyclerSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer BackPackSlotsDisplayer { get; private set; }
    [field: SerializeField] public Storage CharacterInventory { get; private set; }

    [field: SerializeField] public VehiclesController VehiclesController { get; private set; }

    public Item ActiveItem { get; private set; }
    public SlotDisplayer ActiveSlotDisplayer { get; set; }

    private async void Start()
    {
        await Task.Delay(1000);
        if(!IsOwner) return;
        singleton = this;
    }

    public void DisplayInventoryCells()
        => CharacterInventory.Open(this);

    public void SetActiveItem(Item item)
        => ActiveItem = item;

    public void RemoveActiveSlotDisplayer()
    {
        var cell = ActiveSlotDisplayer.ItemDisplayer.InventoryCell;
        CharacterInventory.RemoveItemCountFromSlotServerRpc(ActiveSlotDisplayer.Index, cell.Item.Id, cell.Count);
    }
}
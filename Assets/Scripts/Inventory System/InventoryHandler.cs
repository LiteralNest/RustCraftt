using System.Threading.Tasks;
using ArmorSystem.Backend;
using CharacterStatsSystem;
using Inventory_System;
using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items.Abstract;
using OnPlayerItems;
using Player_Controller;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

public class InventoryHandler : NetworkBehaviour
{
    public static InventoryHandler singleton { get; set; }

    [field: SerializeField] public InventoryPanelsDisplayer InventoryPanelsDisplayer { get; private set; }
    [field: SerializeField] public PlayerNetCode PlayerNetCode { get; private set; }
    [field: SerializeField] public CharacterStatsHandler StatsHandler { get; private set; }
    [field: SerializeField] public PlayerObjectsDragger PlayerObjectsDragger { get; private set; }
    [field: SerializeField] public InHandObjectsContainer InHandObjectsContainer { get; private set; }
    [field: SerializeField] public ArmorsContainer ArmorsContainer { get; private set; }

    [field: SerializeField] public Storage CharacterInventory { get; private set; }

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
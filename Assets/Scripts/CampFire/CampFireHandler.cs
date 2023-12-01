using System.Collections;
using System.Collections.Generic;
using Inventory_System;
using Storage_System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CampFireHandler : Storage
{
    public NetworkVariable<bool> Flaming { get; private set; } = new(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [Header("Main Params")] [SerializeField]
    private GameObject _fireObject;

    [Header("Range")] [SerializeField] private Vector2Int _fuelSlotsRange;
    [SerializeField] private Vector2Int _inputSlotsRange;
    [SerializeField] private Vector2Int _outputSlotsRange;

    [Header("Sound")] [SerializeField] private AudioSource _source;

    private CookingCharacterStatRiser _currentlyCookingCharacterStatRiser;

    private void Start()
        => gameObject.tag = "CampFire";

    private bool IsInRange(int value, Vector2 range)
        => value >= range.x && value < range.y;

    public override bool CanAddItem(Item item, int index)
    {
        if (IsInRange(index, _outputSlotsRange)) return false;
        if (IsInRange(index, _fuelSlotsRange) && item is Fuel) return true;
        if (IsInRange(index, _inputSlotsRange) && item is CookingCharacterStatRiser) return true;
        return false;
    }

    public override void OnNetworkSpawn()
    {
        Flaming.OnValueChanged += (bool prevValue, bool newValue) => { TurnFire(); };
        TurnFire();
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        TryCook();
    }

    public override void Open(InventoryHandler handler)
    {
        handler.InventoryPanelsDisplayer.OpenCampFirePanel();
        SlotsDisplayer = handler.CampFireSlotsDisplayer;
        base.Open(handler);
    }

    private void TurnFire()
        => _fireObject.SetActive(Flaming.Value);

    private List<Fuel> GetFuel()
    {
        List<Fuel> res = new List<Fuel>();
        var cells = ItemsNetData.Value.Cells;
        for (int i = _fuelSlotsRange.x; i < _fuelSlotsRange.y; i++)
        {
            if (cells[i].Id == -1) continue;
            var item = ItemFinder.singleton.GetItemById(cells[i].Id);
            if (item is Fuel && cells[i].Count > 0)
                res.Add(item as Fuel);
        }

        return res;
    }

    private IEnumerator RemoveFuel(Fuel fuel)
    {
        var cells = ItemsNetData.Value.Cells;
        RemoveItemCountServerRpc(fuel.Id, 1);
        SlotsDisplayer.DisplayCells();

        yield return new WaitForSeconds(fuel.BurningTime);
        if (Flaming.Value)
        {
            var fuelList = GetFuel();
            if (fuelList.Count != 0)
                StartCoroutine(RemoveFuel(fuelList[0]));
            else
                TurnFlamingServerRpc(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnFlamingServerRpc(bool value)
    {
        List<Fuel> items = new List<Fuel>();
        Flaming.Value = value;
        if (value)
        {
            items = GetFuel();
            if (items.Count == 0) return;
        }
        else
        {
            _source.Stop();
            return;
        }

        _source.Play();
        StartCoroutine(RemoveFuel(items[0]));
    }

    private List<InventoryCell> GetCookingMaterials()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        var cells = ItemsNetData.Value.Cells;
        for (int i = _inputSlotsRange.x; i < _inputSlotsRange.y; i++)
        {
            if (cells[i].Id == -1) continue;
            var item = ItemFinder.singleton.GetItemById(cells[i].Id);
            if (item is CookingCharacterStatRiser)
                res.Add(new InventoryCell(item, cells[i].Count));
        }

        return res;
    }

    private void TryCook()
    {
        if (!Flaming.Value) return;
        if (_currentlyCookingCharacterStatRiser != null) return;
        var foodList = GetCookingMaterials();
        if (foodList.Count == 0) return;
        var food = foodList[0].Item as CookingCharacterStatRiser;
        var bindedCellItemId = InventoryHelper.GetDesiredCellId(food.CharacterStatRiserAfterCooking.Id, 1, ItemsNetData,
            _outputSlotsRange);
        StartCoroutine(Cook(food, bindedCellItemId));
    }

    private IEnumerator Cook(CookingCharacterStatRiser characterStatRiser, int bindedCellId)
    {
        _currentlyCookingCharacterStatRiser = characterStatRiser;
        yield return new WaitForSeconds(characterStatRiser.CookingTime);
        if (Flaming.Value)
        {
            RemoveItemCountServerRpc(_currentlyCookingCharacterStatRiser.Id, 1);
            AddItemToDesiredSlotServerRpc(characterStatRiser.CharacterStatRiserAfterCooking.Id, 1);
        }

        _currentlyCookingCharacterStatRiser = null;
    }
}
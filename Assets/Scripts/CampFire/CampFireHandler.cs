using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CampFireHandler : Storage
{
    public NetworkVariable<bool> Flaming { get; private set; } = new(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [Header("Main Params")] [SerializeField]
    private GameObject _fireObject;

    [field: SerializeField] public List<Fuel> AvaliableFuel { get; private set; }
    [field: SerializeField] public List<CookingFood> AvaliableFoodForCooking { get; private set; }

    [Header("Sound")] [SerializeField] private AudioSource _source;

    private CampFireSlotsContainer _targetSlotsContainer;

    private CookingFood _currentlyCookingFood;

    private void Start()
        => gameObject.tag = "CampFire";

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
        handler.OpenCampFirePanel();
        _targetSlotsContainer = handler.CampFireSlotsContainer;
        _targetSlotsContainer.Init(Cells, this);
    }

    public override void ConvertWebData(NetworkListEvent<Vector2Int> changeEvent)
    {
        base.ConvertWebData(changeEvent);
        if (_targetSlotsContainer == null) return;
    }

    private void TurnFire()
        => _fireObject.SetActive(Flaming.Value);

    private bool FuelContains(Item item)
    {
        if (item == null) return false;
        foreach (var fuel in AvaliableFuel)
            if (fuel.Id == item.Id)
                return true;
        return false;
    }

    private List<int> GetFuel()
    {
        List<int> res = new List<int>();
        for (int i = 0; i < Cells.Count; i++)
        {
            if (FuelContains(Cells[i].Item) && Cells[i].Count > 0)
                res.Add(i);
        }

        return res;
    }

    private Fuel GetFuelById(int id)
    {
        foreach (var fuel in AvaliableFuel)
            if (fuel.Id == id)
                return fuel;
        Debug.LogError("Can't find fuel with id: " + id);
        return null;
    }

    private IEnumerator RemoveFuel(int cellId)
    {
        RemoveItemCountServerRpc(cellId, 1);
        _targetSlotsContainer.Cells = Cells;
        _targetSlotsContainer.SlotsDisplayer.DisplayCells();
        yield return new WaitForSeconds(GetFuelById(Cells[cellId].Item.Id).BurningTime);
        if (Flaming.Value)
        {
            var fuel = GetFuel();
            if (fuel.Count != 0)
                StartCoroutine(RemoveFuel(fuel[0]));
            else
                TurnFlamingServerRpc(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnFlamingServerRpc(bool value)
    {
        List<int> fuelIds = new List<int>();
        Flaming.Value = value;
        if (value)
        {
            fuelIds = GetFuel();
            if (fuelIds.Count == 0) return;
        }
        else
        {
            _source.Stop();
            return;
        }

        _source.Play();
        StartCoroutine(RemoveFuel(fuelIds[0]));
    }

    private void RemoveItem(Item item, int count)
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            if (Cells[i].Item == item)
                RemoveItemCountServerRpc(i, count);
        }
    }

    private bool FoodContains(Item item)
    {
        if (item == null) return false;
        foreach (var fuel in AvaliableFoodForCooking)
            if (fuel.Id == item.Id)
                return true;
        return false;
    }

    private CookingFood GetFoodById(int id)
    {
        foreach (var food in AvaliableFoodForCooking)
            if (food.Id == id)
                return food;
        Debug.LogError("Can't find food with id: " + id);
        return null;
    }

    private List<InventoryCell> GetFood()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var cell in Cells)
        {
            if (FoodContains(cell.Item))
                res.Add(cell);
        }

        return res;
    }

    private void TryCook()
    {
        if (!Flaming.Value) return;
        if (_currentlyCookingFood != null) return;
        var foodList = GetFood();
        if (foodList.Count == 0) return;
        var food = GetFoodById(foodList[0].Item.Id);
        var bindedCell = InventoryHelper.GetDesiredCell(food.FoodAfterCooking, 1, Cells);
        if (bindedCell == null) return;
        StartCoroutine(Cook(food, bindedCell));
    }

    private IEnumerator Cook(CookingFood food, InventoryCell bindedCell)
    {
        _currentlyCookingFood = food;
        yield return new WaitForSeconds(food.CookingTime);
        if (Flaming.Value)
        {
            RemoveItem(_currentlyCookingFood, 1);
            bindedCell.Item = food.FoodAfterCooking;
            bindedCell.Count += 1;
        }

        _currentlyCookingFood = null;
    }
}
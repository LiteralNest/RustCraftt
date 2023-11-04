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

    public override void ConvertWebData(NetworkListEvent<Vector2> changeEvent)
    {
        base.ConvertWebData(changeEvent);
        if (_targetSlotsContainer == null) return;
    }

    private void TurnFire()
        => _fireObject.SetActive(Flaming.Value);

    private List<int> GetFuel()
    {
        List<int> res = new List<int>();
        for (int i = 0; i < Cells.Count; i++)
        {
            if (Cells[i].Item is Fuel && Cells[i].Count > 0)
                res.Add(i);
        }

        return res;
    }

    private IEnumerator RemoveFuel(int cellId)
    {
        RemoveItemCountServerRpc(cellId, 1);
        _targetSlotsContainer.Cells = Cells;
        _targetSlotsContainer.SlotsDisplayer.DisplayCells();
        var currentFuel = Cells[cellId].Item as Fuel;
        yield return new WaitForSeconds(currentFuel.BurningTime);
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
    
    private List<InventoryCell> GetFood()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var cell in Cells)
        {
            if (cell.Item is CookingFood)
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
        var food = foodList[0].Item as CookingFood;
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
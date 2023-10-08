using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CampFireHandler : NetworkBehaviour
{
    public NetworkVariable<bool> Flaming { get; private set; } = new(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [Header("Main Params")] [SerializeField]
    private GameObject _fireObject;

    [field: SerializeField] public List<Fuel> AvaliableFuel { get; private set; }
    [field: SerializeField] public List<CookingFood> AvaliableFoodForCooking { get; private set; }

    [field: SerializeField] public List<InventoryCell> Cells { get; private set; }

    private CampFireSlotsContainer _targetSlotsContainer;
    
    private CookingFood _currentlyCookingFood;
    
    private void Start()
    {
        TurnFire();
        Flaming.OnValueChanged += (bool prevValue, bool newValue) => { TurnFire(); };
    }

    private void Update()
    {
        TryCook();
    }
    
    public void SetItem(int index, InventoryCell cell)
    {
        Cells[index].Item = cell.Item;
        Cells[index].Count = cell.Count;
    }

    private void SetItemCount(int index, int count)
    {
        Cells[index].Count = count;
    }

    public void Open(InventoryHandler handler)
    {
        handler.OpenCampFirePanel();
        _targetSlotsContainer = handler.CampFireSlotsContainer;
        _targetSlotsContainer.Init(this);
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

    private List<InventoryCell> GetFuel()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var cell in Cells)
        {
            if (FuelContains(cell.Item))
                res.Add(cell);
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

    private IEnumerator RemoveFuel(InventoryCell cell)
    {
        cell.Count--;
        _targetSlotsContainer.SlotsDisplayer.DisplayCells();
        yield return new WaitForSeconds(GetFuelById(cell.Item.Id).BurningTime);
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
        List<InventoryCell> fuel = new List<InventoryCell>();
        if (value)
        {
            fuel = GetFuel();
            if (fuel.Count == 0) return;
        }
        Flaming.Value = value;
        if (!value) return;
        StartCoroutine(RemoveFuel(fuel[0]));
    }

    private InventoryCell GetFreeCell()
    {
        foreach (var cell in Cells)
            if (cell.Item == null)
                return cell;
        return null;
    }

    private InventoryCell GetDesiredCell(Item item, int count)
    {
        foreach (var cell in Cells)
        {
            if (cell.Item == item)
            {
                if (cell.Item.StackCount >= count + cell.Count)
                    return cell;
            }
        }
        return GetFreeCell(); 
    }

    private void ResetCell(InventoryCell cell)
    {
        cell.Item = null;
        cell.Count = 0;
        _targetSlotsContainer.SlotsDisplayer.DisplayCells();
    }
    
    private void RemoveItem(Item item, int count)
    {
        foreach (var cell in Cells)
        {
            if (cell.Item == item)
            {
                cell.Count -= count;
                if(cell.Count > 0) return;
                ResetCell(cell);
            }
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
        if(!Flaming.Value) return;
        if(_currentlyCookingFood != null) return;
        var foodList = GetFood();
        if(foodList.Count == 0) return;
        var food = GetFoodById(foodList[0].Item.Id);
        var bindedCell = GetDesiredCell(food.FoodAfterCooking, 1);
        if(bindedCell == null) return;
        StartCoroutine(Cook(food, bindedCell));
    }
    
    private IEnumerator Cook(CookingFood food, InventoryCell bindedCell)
    {
        _currentlyCookingFood = food;
        yield return new WaitForSeconds(food.CookingTime);
        if(Flaming.Value)
        {
            RemoveItem(_currentlyCookingFood, 1);
            bindedCell.Item = food.FoodAfterCooking;
            bindedCell.Count += 1;
        }
        _currentlyCookingFood = null;
    }
}
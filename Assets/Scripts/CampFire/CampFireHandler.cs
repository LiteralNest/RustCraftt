using System.Collections;
using System.Collections.Generic;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

public class CampFireHandler : Storage
{
    public NetworkVariable<bool> Flaming { get; private set; } = new(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [Header("Main Params")] [SerializeField]
    private GameObject _fireObject;

    [Header("Sound")] 
    [SerializeField] private AudioSource _source;

    private CookingCharacterStatRiser _currentlyCookingCharacterStatRiser;

    private void Start()
        => gameObject.tag = "CampFire";

    public override bool CanAddItem(Item item)
    {
        if(item is CharacterStatRiser || item is CookingCharacterStatRiser) return true;
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

    private List<int> GetFuel()
    {
        List<int> res = new List<int>();
        var cells = ItemsNetData.Value.Cells;
        for (int i = 0; i < cells.Length; i++)
        {
            var item = ItemFinder.singleton.GetItemById(cells[i].Id);
            if (item is Fuel && cells[i].Count > 0)
                res.Add(i);
        }

        return res;
    }

    private IEnumerator RemoveFuel(int cellId)
    {
        RemoveItem(cellId, 1);
        var cells = ItemsNetData.Value.Cells;
        var item = ItemFinder.singleton.GetItemById(cells[cellId].Id);
        var currentFuel = item as Fuel;
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

    private List<InventoryCell> GetFood()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var cell in ItemsNetData.Value.Cells)
        {
            var item = ItemFinder.singleton.GetItemById(cell.Id);
            if (item is CookingCharacterStatRiser)
                res.Add(new InventoryCell(item, cell.Count));
        }
        return res;
    }

    private void TryCook()
    {
        if (!Flaming.Value) return;
        if (_currentlyCookingCharacterStatRiser != null) return;
        var foodList = GetFood();
        if (foodList.Count == 0) return;
        var food = foodList[0].Item as CookingCharacterStatRiser;
        var bindedCellItemId = InventoryHelper.GetDesiredCellId(food.CharacterStatRiserAfterCooking.Id, 1, ItemsNetData);
        StartCoroutine(Cook(food, bindedCellItemId));
    }

    private IEnumerator Cook(CookingCharacterStatRiser characterStatRiser, int bindedCellId)
    {
        _currentlyCookingCharacterStatRiser = characterStatRiser;
        yield return new WaitForSeconds(characterStatRiser.CookingTime);
        if (Flaming.Value)
        {
            RemoveItem(_currentlyCookingCharacterStatRiser, 1);
            AddItem(bindedCellId, characterStatRiser.CharacterStatRiserAfterCooking.Id, 1);
        }

        _currentlyCookingCharacterStatRiser = null;
    }
}
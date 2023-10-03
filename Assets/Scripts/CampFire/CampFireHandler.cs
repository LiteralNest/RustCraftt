using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CampFireHandler : NetworkBehaviour
{
    [Header("Web")] [SerializeField] private NetworkVariable<bool> _flaming = new(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [Header("AttachedScripts")] [SerializeField]
    private List<InventoryCell> _cells;

    [Header("Main Params")]
    [SerializeField] private List<Item> _avaliableFuel;
    [SerializeField] private List<CookingFood> _avaliableFoodForCooking;

    [SerializeField] private GameObject _fireObject;

    private CampFireSlotsContainer _targetSlotsContainer;

    private void Start()
    {
        TurnFire();
        _flaming.OnValueChanged += (bool prevValue, bool newValue) =>
        {
            TurnFire();
        };
    }

    private void SetItemCount(int index, int count)
    {
        _cells[index].Count = count;
    }

    private int TrySetItemToSimilar(Item item, int count)
    {
        int currentCount = count;

        int index = -1;
        foreach (var cell in _cells)
        {
            index++;
            if (cell.Item == null || cell.Item.Id != item.Id) continue;
            int sum = currentCount + cell.Count;
            if (sum < cell.Item.StackCount)
            {
                SetItemCount(index, sum);
                return 0;
            }

            currentCount = sum - cell.Item.StackCount;
            SetItemCount(index, cell.Item.StackCount);
        }

        return currentCount;
    }

    private int GetFreeItemCellIndex()
    {
        int index = 0;
        foreach (var cell in _cells)
        {
            if (cell.Item == null) return index;
            index++;
        }

        return -1;
    }

    public void AddCell(InventoryCell addingCell)
    {
        int resCount = TrySetItemToSimilar(addingCell.Item, addingCell.Count);
        if (resCount > 0)
        {
            int cellIndex = GetFreeItemCellIndex();
            _cells[cellIndex] = new InventoryCell(addingCell.Item, resCount);
        }
    }

    public void RemoveCell(InventoryCell removingCell)
    {
        foreach (var cell in _cells)
        {
            if (cell.Item == null || removingCell.Item == null) return;
            if (cell.Item.Id == removingCell.Item.Id && cell.Count == removingCell.Count)
            {
                cell.Item = null;
                cell.Count = 0;
                return;
            }
        }
    }

    public void Open(InventoryHandler handler)
    {
        handler.OpenCampFirePanel();
        handler.CampFireSlotsContainer.ResetCells();
        _targetSlotsContainer = handler.CampFireSlotsContainer;
        _targetSlotsContainer.Init(this, _cells);
    }

    private void TurnFire()
        => _fireObject.SetActive(_flaming.Value);
    
    [ServerRpc(RequireOwnership = false)]
    public void TurnFlamingServerRpc(bool value)
        => _flaming.Value = value;
}
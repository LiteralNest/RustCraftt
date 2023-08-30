using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingQueue : MonoBehaviour
{
    [Header("UI")] [SerializeField] private CreatingQueueAlertDisplayer _alertDisplayer;
    [SerializeField] private Transform _placeForCells;
    [SerializeField] private CraftingQueueCellDisplayer _cellPrefab;


    private List<CraftingQueueCellDisplayer> _displayers = new List<CraftingQueueCellDisplayer>();

    private void CheckForCreatingCells()
    {
        foreach (var cell in _displayers)
            if (cell.Creating)
                return;
        if (_displayers.Count <= 0) return;
        _displayers[0].CreateItems();
    }

    public void CreateCell(CraftingItem item, int count, List<CraftingItemDataTableSlot> slots)
    {
        var instance = Instantiate(_cellPrefab, _placeForCells);
        instance.Init(item, count, this, slots);
        _displayers.Add(instance);
        CheckForCreatingCells();
    }

    public void DeleteCell(CraftingQueueCellDisplayer cell)
    {
        _displayers.Remove(cell);
        CheckForCreatingCells();
    }

    public void DisplayAlert(bool value)
    {
        if (_alertDisplayer.gameObject.activeSelf != value)
            _alertDisplayer.gameObject.SetActive(value);
    }
    
    public void DisplayAlert(Item item, int count, int time)
    {
        DisplayAlert(true);
        _alertDisplayer.Init(item, count, time);
    }
}
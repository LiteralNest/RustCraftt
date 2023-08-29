using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CraftingItemDataDisplayer))]
public class CraftingSlotsContainer : MonoBehaviour
{
    [Header("Attached Scripts")] 
    [SerializeField] private CraftingItemDataDisplayer _craftingItemDataDisplayer;
    [Header("UI")] 
    [SerializeField] private CraftingSlotDisplayer _craftingSlotPrefab;
    [SerializeField] private Transform _placeForSlots;

    [field: SerializeField] public List<CraftingItem> CraftingItems { get; private set; }

    private void Start()
    {
        if (_craftingItemDataDisplayer == null)
            _craftingItemDataDisplayer = GetComponent<CraftingItemDataDisplayer>();
        DisplaySlots();
    }
    
    private void ClearPlace(Transform place)
    {
        foreach (Transform child in place)
            Destroy(child.gameObject);
    }

    private void DisplaySlots()
    {
        ClearPlace(_placeForSlots);
        foreach (var item in CraftingItems)
            Instantiate(_craftingSlotPrefab, _placeForSlots).Init(item, _craftingItemDataDisplayer);
    }
}
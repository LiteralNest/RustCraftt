using Crafting_System.Crafting_Slots;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CraftingItemDataTableSlotsContainer))]
public class CraftingItemDataDisplayer : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CraftingItemDataTableSlotsContainer _slotsContainer;
    
    [Header("UI")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _creatingTimeText;
    [SerializeField] private Image _iconImage;

    public CraftingItem CurrentItem { get; private set; }

    private void Start()
    {
        if (_slotsContainer == null)
            _slotsContainer = GetComponent<CraftingItemDataTableSlotsContainer>();
    }
    
    public void DisplayData(CraftingItem item)
    {
        _titleText.text = item.Name;
        _descriptionText.text = item.Description;
        _creatingTimeText.text = item.TimeForCreating + "s";
        _slotsContainer.AddSlots(item.NeededSlots);
        _iconImage.sprite = item.Icon;
        CurrentItem = item;
    }
}

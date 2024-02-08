using Crafting_System.Crafting_Item_Data_Displayer.CraftingItemDataTable;
using Items_System.Items.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting_System.Crafting_Item_Data_Displayer
{
    [RequireComponent(typeof(CraftingItemDataTableSlotsContainer))]
    public class CraftingItemDataDisplayer : MonoBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private CraftingItemDataTableSlotsContainer _slotsContainer;

        [Header("UI")] [SerializeField] private GameObject _itemInfoPanel;
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

        public void HandleInfoPanel(bool value)
            => _itemInfoPanel.SetActive(value);

        public void DisplayData(CraftingItem item)
        {
            HandleInfoPanel(true);
            _titleText.text = item.Name;
            _descriptionText.text = item.Description;
            _creatingTimeText.text = item.TimeForCreating + "s";
            _slotsContainer.AddSlots(item.NeededSlots);
            _iconImage.sprite = item.Icon;
            CurrentItem = item;
        }
    }
}
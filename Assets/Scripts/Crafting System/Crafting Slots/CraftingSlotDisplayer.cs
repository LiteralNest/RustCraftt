using Items_System.Items.Abstract;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CraftingSlotDisplayer : MonoBehaviour, IPointerDownHandler
{
    [Header("UI")] [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Image _icon;

    private CraftingItemDataDisplayer _dataDisplayer;
    private CraftingItem _item;

    public void Init(CraftingItem item, CraftingItemDataDisplayer dataDisplayer)
    {
        _item = item;
        _dataDisplayer = dataDisplayer;
        _titleText.text = item.Name;
        _icon.sprite = item.Icon;
    }

    private void DisplayData()
        => _dataDisplayer.DisplayData(_item);

    public void OnPointerDown(PointerEventData eventData)
        => DisplayData();
}
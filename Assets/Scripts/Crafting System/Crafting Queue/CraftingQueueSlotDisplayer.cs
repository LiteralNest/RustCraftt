using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingQueueSlotDisplayer : MonoBehaviour, IPointerDownHandler
{
    [Header("UI")] [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _displayingIcon;

    private CraftingQueueSlotFunctional _craftingQueueSlotFunctional;

    public void Init(CraftingItem craftingItem, int count, CraftingQueueSlotFunctional craftingQueueSlotFunctional)
    {
        _craftingQueueSlotFunctional = craftingQueueSlotFunctional;
        _displayingIcon.sprite = craftingItem.Icon;
        DisplayCountText(count);
    }

    public void DisplayTimeText(int time)
        => _timeText.text = time + "s";


    public void DisplayCountText(int count)
        => _countText.text = count.ToString();

    public void OnPointerDown(PointerEventData eventData)
    {
        _craftingQueueSlotFunctional.Delete(true);
    }
}
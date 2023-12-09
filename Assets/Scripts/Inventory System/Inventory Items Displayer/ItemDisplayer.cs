using System.Collections;
using System.Threading.Tasks;
using Storage_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemDisplayer : MonoBehaviour
{
    [Header("UI")] [SerializeField] protected TMP_Text _countText;
    [SerializeField] protected Image _itemIcon;
    [Header("Animator")] [SerializeField] protected Animator _animator;
    [SerializeField] private AnimationClip _movingClip;

    public InventoryCell InventoryCell { get; protected set; }
    public SlotDisplayer PreviousCell { get; protected set; }

    public virtual void MinusCurrentHp(int hp)
    {
    }

    public virtual void MinusCurrentAmmo(int value)
    {
    }

    public virtual void SetCurrentAmmo(int value)
    {
    }

    public virtual int GetCurrentHp()
        => 0;

    public void SetInventoryCell(InventoryCell inventoryCell)
    {
        InventoryCell = inventoryCell;
        DisplayData();
    }

    public virtual void DisplayData()
    {
        if (InventoryCell.Item == null) return;
        if (InventoryCell.Item.StackCount == 1)
            _countText.gameObject.SetActive(false);
        _itemIcon.sprite = InventoryCell.Item.Icon;
        _countText.text = InventoryCell.Count.ToString();
    }

    public virtual int StackCount(ItemDisplayer displayer)
    {
        var cell = displayer.InventoryCell;
        var currentItemCount = InventoryCell.Count;
        int count = currentItemCount + cell.Count;
        if (count <= InventoryCell.Item.StackCount)
        {
            InventoryCell.Count = count;
            PreviousCell.Inventory.SetItemAndResetCellServerRpc(PreviousCell.Index,
                new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count, InventoryCell.Hp,
                    InventoryCell.Ammo), displayer.PreviousCell.Index);
            Destroy(displayer.gameObject);
            return 0;
        }

        InventoryCell.Count = InventoryCell.Item.StackCount;
        cell.Count = count - InventoryCell.Item.StackCount;
        return count - InventoryCell.Item.StackCount;
    }

    public void SetPosition()
        => transform.position = PreviousCell.transform.position;

    public virtual void SetNewCell(SlotDisplayer slotDisplayer)
    {
        PreviousCell = slotDisplayer;
        var slotTransform = slotDisplayer.transform;
        transform.SetParent(slotTransform);
        SetPosition();
    }

    public async void MoveToOtherInventory()
    {
        _animator.SetTrigger("Moving");
        await Task.Delay((int)(_movingClip.length * 1000));
        ActiveInvetoriesHandler.singleton.HandleCell(this);
    }
}
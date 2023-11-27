using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemDisplayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected TMP_Text _countText;
    [SerializeField] protected Image _itemIcon;
    [Header("Animator")]
    [SerializeField] protected Animator _animator;
    [SerializeField] private AnimationClip _movingClip;
    
    public InventoryCell InventoryCell { get; protected set; }
    public SlotDisplayer PreviousCell { get; protected set; }

    public virtual void MinusCurrentHp(int hp){}
    public virtual void AddCurrentHp(int hp){}

    public virtual int GetCurrentHp()
        => 0;

    public void SetInventoryCell(InventoryCell inventoryCell)
    {
        InventoryCell = inventoryCell;
        DisplayData();
    }
    
    public virtual void DisplayData()
    {
        if(InventoryCell.Item == null) return;
        _itemIcon.sprite = InventoryCell.Item.Icon;
        _countText.text = InventoryCell.Count.ToString();
    }

    public virtual int StackCount(int addedCount, SlotDisplayer slotDisplayer)
    {
        var currentItemCount = InventoryCell.Count;
        int count = currentItemCount + addedCount;
        if (count <= InventoryCell.Item.StackCount)
        {
            InventoryCell.Count = count;
            DisplayData();
            return 0;
        }

        InventoryCell.Count = InventoryCell.Item.StackCount;
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
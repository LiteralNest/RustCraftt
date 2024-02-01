using Inventory_System.Inventory_Slot_Displayers;
using Player_Controller;
using Storage_System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory_System.Inventory_Items_Displayer
{
    public abstract class ItemDisplayer : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI")] [SerializeField] protected TMP_Text _countText;
        [SerializeField] protected Image _itemIcon;
        [Header("Animator")] [SerializeField] protected Animator _animator;

        public InventoryCell InventoryCell { get; protected set; }
        public SlotDisplayer PreviousCell { get; protected set; }

        public void OnPointerClick(PointerEventData eventData)
            => PointerClicked();

        protected virtual void PointerClicked()
        {
            if (PlayerNetCode.Singleton.ItemInfoHandler)
                PlayerNetCode.Singleton.ItemInfoHandler.AssignItem(PreviousCell);
        }
        public virtual void MinusCurrentHp(int hp)
        {
        }

        public virtual void MinusCurrentAmmo(int value)
        {
        }

        public virtual void SetCurrentAmmo(int value)
        {
        }
    
        public virtual void DoAfterMovingItemOut()
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

        public void StackCount(ItemDisplayer displayer)
        {
            int sum = InventoryCell.Count + displayer.InventoryCell.Count;
            if (sum <= InventoryCell.Item.StackCount)
            {
                displayer.PreviousCell.Inventory.ResetItemServerRpc(displayer.PreviousCell.Index, (int)PlayerNetCode.Singleton.OwnerClientId);
                PreviousCell.Inventory.SetItemServerRpc(PreviousCell.Index, new CustomSendingInventoryDataCell(
                    InventoryCell.Item.Id, sum, InventoryCell.Hp,
                    InventoryCell.Ammo));
            }
            else
            {
                displayer.PreviousCell.Inventory.SetItemServerRpc(displayer.PreviousCell.Index,
                    new CustomSendingInventoryDataCell(
                        InventoryCell.Item.Id, sum - InventoryCell.Item.StackCount, InventoryCell.Hp, InventoryCell.Ammo
                    ));
                PreviousCell.Inventory.SetItemServerRpc(PreviousCell.Index, new CustomSendingInventoryDataCell(
                    InventoryCell.Item.Id, InventoryCell.Item.StackCount, InventoryCell.Hp,
                    InventoryCell.Ammo));
            }
        }

        public void SetPosition()
            => transform.position = PreviousCell.transform.position;

        public void SetNewCell(SlotDisplayer slotDisplayer)
        {
            PreviousCell = slotDisplayer;
            var slotTransform = slotDisplayer.transform;
            transform.SetParent(slotTransform);
            SetPosition();
        }

        public void StartMovingToOtherInventory()
            => _animator.SetTrigger("Moving");

        public void MoveToOtherInventory()
            => ActiveInvetoriesHandler.singleton.HandleCell(this);
    }
}
using Inventory_System.Inventory_Items_Displayer;
using Storage_System;
using UnityEngine;
using UnityEngine.InputSystem.WebGL;
using UnityEngine.UI;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public class DamagableItemDisplayer : InventoryItemDisplayer
    {
        [SerializeField] private Image _hpBar;
        [SerializeField] private GameObject _hpBarParent;
        private DamagableItem _currentItem;

        public override void DisplayData()
        {
            if (InventoryCell.Item == null) return;
            var item = InventoryCell.Item as DamagableItem;
            _currentItem = item;
            InventoryCell.Hp = item.Hp;
            if (InventoryCell.Hp <= 0)
            {
                if (PreviousCell != null)
                {
                    InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                        new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                            InventoryCell.Hp));
                }
                DisplayBar(InventoryCell.Hp);
            }
            DisplayBar(InventoryCell.Hp);
        }

        public void DisplayBar(int hp)
        {
            if(_currentItem.Hp <= 0)
            {
                _hpBarParent.SetActive(false);
                return;
            }
            _hpBar.fillAmount = (float)hp / _currentItem.Hp;
        }

        public override void MinusCurrentHp(int hp)
        {
            var item = InventoryCell.Item as Tool;
            if(item.Hp <= 0) return;
            InventoryCell.Hp -= hp;

            if (InventoryCell.Hp <= 0)
            {
                InventoryHandler.singleton.CharacterInventory.ResetItemServerRpc(PreviousCell.Index);
                return;
            }

            InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                    InventoryCell.Hp));
            DisplayBar(InventoryCell.Hp);
        }

        public override int GetCurrentHp()
            => InventoryCell.Hp;

        public override void AddCurrentHp(int hp)
        {
            InventoryCell.Hp += hp;
            if (_storage != null)
            {
                InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                    new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                        InventoryCell.Hp));
                DisplayBar(InventoryCell.Hp);
            }
        }

        [ContextMenu("Test Minusing Hp")]
        private void Test()
        {
            MinusCurrentHp(10);
        }
    }
}
using Inventory_System.Inventory_Items_Displayer;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public class DamagableItemDisplayer : InventoryItemDisplayer
    {
        [SerializeField] private Image _hpBar;
        private DamagableItem _currentItem;

        public override void DisplayData()
        {
            if (InventoryCell.Item == null) return;
            var item = InventoryCell.Item as DamagableItem;
            if (InventoryCell.Hp <= 0)
            {
                InventoryCell.Hp = item.Hp;
                _storage.SetItemServerRpc(PreviousCell.Index, InventoryCell.Item.Id, InventoryCell.Count,
                    InventoryCell.Hp, true, false);
            }

            _currentItem = item;
            DisplayBar(InventoryCell.Hp);
        }

        public void DisplayBar(int hp)
            => _hpBar.fillAmount = (float)hp / _currentItem.Hp;

        public override void MinusCurrentHp(int hp)
        {
            InventoryCell.Hp -= hp;

            if (InventoryCell.Hp <= 0)
            {
                InventoryHandler.singleton.CharacterInventory.ResetItemServerRpc(PreviousCell.Index);
                return;
            }

            InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                InventoryCell.Item.Id,
                InventoryCell.Count, InventoryCell.Hp, true, false);
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
                    InventoryCell.Item.Id, InventoryCell.Count, InventoryCell.Hp, true, false);
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
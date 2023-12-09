using Inventory_System;
using Items_System.Items;
using Items_System.Items.Abstract;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MeltingSystem
{
    public class CampFire : Smelter
    {
        protected CookingCharacterStatRiser _currentlyCookingCharacterStatRiser;
        
        public override bool CanAddItem(Item item, int index)
        {
            if (IsInRange(index, _outputSlotsRange)) return false;
            if (IsInRange(index, _fuelSlotsRange) && item is Fuel) return true;
            if (IsInRange(index, _inputSlotsRange) && item is CookingCharacterStatRiser) return true;
            return false;
        }

        protected override void Cook()
        {
            TryCook();
        }
        
        private List<InventoryCell> GetCookingMaterials()
        {
            List<InventoryCell> res = new List<InventoryCell>();
            var cells = ItemsNetData.Value.Cells;
            for (int i = _inputSlotsRange.x; i < _inputSlotsRange.y; i++)
            {
                if (cells[i].Id == -1) continue;
                var item = ItemFinder.singleton.GetItemById(cells[i].Id);
                if (item is CookingCharacterStatRiser)
                    res.Add(new InventoryCell(item, cells[i].Count));
            }

            return res;
        }
        
        private void TryCook()
        {
            if (!Flaming.Value) return;
            if (_currentlyCookingCharacterStatRiser != null) return;
            var foodList = GetCookingMaterials();
            if (foodList.Count == 0) return;
            var food = foodList[0].Item as CookingCharacterStatRiser;
            StartCoroutine(Cook(food));
        }

        private IEnumerator Cook(CookingCharacterStatRiser characterStatRiser)
        {
            _currentlyCookingCharacterStatRiser = characterStatRiser;
            yield return new WaitForSeconds(characterStatRiser.CookingTime);
            if (Flaming.Value)
            {
                RemoveItemCountServerRpc(_currentlyCookingCharacterStatRiser.Id, 1);
                AddItemToDesiredSlotServerRpc(characterStatRiser.CharacterStatRiserAfterCooking.Id, 1, 0,_outputSlotsRange);
            }

            _currentlyCookingCharacterStatRiser = null;
        }
    }
}

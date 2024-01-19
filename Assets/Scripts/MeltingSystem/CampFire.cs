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
        protected CookingFood CurrentlyCookingFood;
        
        public override bool CanAddItem(Item item, int index)
        {
            if (IsInRange(index, _outputSlotsRange)) return false;
            if (IsInRange(index, _fuelSlotsRange) && item is Fuel) return true;
            if (IsInRange(index, _inputSlotsRange) && item is CookingFood) return true;
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
                if (item is CookingFood)
                    res.Add(new InventoryCell(item, cells[i].Count));
            }

            return res;
        }
        
        private void TryCook()
        {
            if (!Flaming.Value) return;
            if (CurrentlyCookingFood != null) return;
            var foodList = GetCookingMaterials();
            if (foodList.Count == 0) return;
            var food = foodList[0].Item as CookingFood;
            StartCoroutine(Cook(food));
        }

        private IEnumerator Cook(CookingFood food)
        {
            CurrentlyCookingFood = food;
            yield return new WaitForSeconds(food.CookingTime);
            if (Flaming.Value)
            {
                RemoveItemCountServerRpc(CurrentlyCookingFood.Id, 1);
                AddItemToDesiredSlotServerRpc(food.FoodAfterCooking.Id, 1, 0,0,_outputSlotsRange);
            }

            CurrentlyCookingFood = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Items_System.Items;
using Items_System.Items.Abstract;
using UnityEngine;

namespace MeltingSystem
{
    public class Furnace : Smelter
    {
        private MeltingOre _currentMeltingOre;
        
        public override bool CanAddItem(Item item, int index)
        {
            if (IsInRange(index, _outputSlotsRange)) return false;
            if (IsInRange(index, _fuelSlotsRange) && item is Fuel) return true;
            if (IsInRange(index, _inputSlotsRange) && item is MeltingOre) return true;
            return false;
        }
        
        public override void Open(InventoryHandler handler)
        {
            handler.InventoryPanelsDisplayer.OpenFurnacePanel();
            SlotsDisplayer = handler.FurnaceSlotsDiaplayer;
            base.Open(handler);
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
                if (item is MeltingOre)
                    res.Add(new InventoryCell(item, cells[i].Count));
            }

            return res;
        }
        
        private void TryCook()
        {
            if (!Flaming.Value) return;
            if (_currentMeltingOre != null) return;
            var foodList = GetCookingMaterials();
            if (foodList.Count == 0) return;
            var ore = foodList[0].Item as MeltingOre;
            StartCoroutine(Cook(ore));
        }

        private IEnumerator Cook(MeltingOre meltingOre)
        {
            _currentMeltingOre = meltingOre;
            yield return new WaitForSeconds(meltingOre.MeltingTime);
            if (Flaming.Value)
            {
                RemoveItemCountServerRpc(_currentMeltingOre.Id, 1);
                AddItemToDesiredSlotServerRpc(meltingOre.Result.Id, 1,0, _outputSlotsRange);
            }

            _currentMeltingOre = null;
        }
    }
}

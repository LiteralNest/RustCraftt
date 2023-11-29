using TMPro;
using UnityEngine;

namespace Inventory_System.Slots_Displayer.Tool_CLipBoard
{
    public class ToolClipBoardSlotsDisplayer : SlotsDisplayer
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private GameObject _thereIsItemsObject;
        [SerializeField] private GameObject _noItemsObject;
        
        [Header("Cells for 24 hours")]
        [SerializeField] private ToolClipBoardDisplayingCellsDisplayer _cellsDisplayer;
        
        public override void InitItems()
        {
            foreach (var cell in CellDisplayers)
                cell.CanSetSlot = true;
        }

        private void DisplayPanels(bool enoughMaterials)
        {
            _thereIsItemsObject.SetActive(enoughMaterials);
            _noItemsObject.SetActive(!enoughMaterials);
        }

        public void DisplayTime(int time)
        {
            int days = time / 24;
            int hours = time % 24;
            if (hours == 0 && days == 0)
            {
                DisplayPanels(false);
                return;
            }

            DisplayPanels(true);
            _timeText.text = "Protected for " + days + "d " + hours + "h";
        }

        public void DisplayRemainingTime()
        {
            var storage = TargetStorage as ToolClipboard;
            DisplayTime(storage.GetAvaliableMinutes());
            _cellsDisplayer.DisplayCells(storage.GetNeededResourcesForDay());
        }
        
        public override void DisplayCells()
        {
            base.DisplayCells();
            DisplayRemainingTime();
        }
    }
}
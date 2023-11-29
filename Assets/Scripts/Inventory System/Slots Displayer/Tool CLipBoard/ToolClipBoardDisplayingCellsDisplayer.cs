using System.Collections.Generic;
using UnityEngine;

namespace Inventory_System.Slots_Displayer.Tool_CLipBoard
{
    public class ToolClipBoardDisplayingCellsDisplayer : MonoBehaviour
    {
        [SerializeField] private Transform _place;
        [SerializeField] private ToolClipBoardDisplayingCell _targetPref;

        private void ClearPlace(Transform place)
        {
            foreach(Transform child in _place)
                Destroy(child.gameObject);
        }

        public void DisplayCells(List<InventoryCell> cells)
        {
            ClearPlace(_place);
            foreach (var cell in cells)
            {
                var instance = Instantiate(_targetPref, _place);
                instance.Init(cell.Item.Icon, cell.Count);
            }
        }
    }
}
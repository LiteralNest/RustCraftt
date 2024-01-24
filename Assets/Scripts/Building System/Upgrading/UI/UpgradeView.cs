using System.Collections.Generic;
using UnityEngine;

namespace Building_System.Upgrading.UI
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private List<UpgradeCellView> _upgradeCells;

        public void DisplayUpgradeCells(List<InventoryCell> cells, int level)
        {
            foreach (var cell in cells)
            {
                foreach (var upgradeCell in _upgradeCells)
                {
                    if (upgradeCell.TargetResource.Id == cell.Item.Id)
                    {
                        upgradeCell.DisplayActive(cell.Count, level);
                        break;
                    }
                }
            }
        }
    }
}
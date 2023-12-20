using Inventory_System;
using UnityEngine;

namespace Crafting_System.WorkBench
{
    public class WorkBench : MonoBehaviour
    {
        public void Open()
            => InventoryPanelsDisplayer.singleton.OpenWorkbenchPanel();
    }
}
using Inventory_System;
using UnityEngine;

namespace Crafting_System.WorkBench
{
    public class WorkBench : MonoBehaviour
    {
        [field: SerializeField] public int Level { get; private set; }

        public void Open()
            => InventoryPanelsDisplayer.singleton.OpenWorkbenchPanel();
    }
}
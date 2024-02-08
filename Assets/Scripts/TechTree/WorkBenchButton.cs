using Inventory_System;
using UnityEngine;
using UnityEngine.UI;

namespace TechTree
{
    [RequireComponent(typeof(Button))]
    public class WorkBenchButton : MonoBehaviour
    {
        [SerializeField] private InventoryPanelsDisplayer _inventoryPanelsDisplayer;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _inventoryPanelsDisplayer.CloseWorkbenchPanel();
            });
        }
    }
}
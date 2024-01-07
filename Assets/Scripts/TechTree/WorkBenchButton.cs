using Inventory_System;
using UnityEngine;
using UnityEngine.UI;

namespace TechTree
{
    [RequireComponent(typeof(Button))]
    public class WorkBenchButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                InventoryPanelsDisplayer.singleton.CloseWorkbenchPanel();
            });
        }
    }
}
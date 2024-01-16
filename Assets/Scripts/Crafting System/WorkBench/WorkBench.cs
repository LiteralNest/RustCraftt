using UnityEngine;

namespace Crafting_System.WorkBench
{
    public class WorkBench : MonoBehaviour
    {
        public void Open()
        {
            InventoryHandler.singleton.InventoryPanelsDisplayer.OpenWorkbenchPanel();
        }
    }
}
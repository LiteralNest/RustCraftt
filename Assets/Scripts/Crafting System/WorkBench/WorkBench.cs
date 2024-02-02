using InteractSystem;
using UnityEngine;

namespace Crafting_System.WorkBench
{
    public class WorkBench : MonoBehaviour, IRaycastInteractable
    {
        public string GetDisplayText()
            => "Open";

        public void Interact()
            => InventoryHandler.singleton.InventoryPanelsDisplayer.OpenWorkbenchPanel();

        public bool CanInteract()
            => true;
    }
}
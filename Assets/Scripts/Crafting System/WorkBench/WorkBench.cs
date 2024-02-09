using InteractSystem;
using Inventory_System;
using UnityEngine;

namespace Crafting_System.WorkBench
{
    public class WorkBench : MonoBehaviour, IRaycastInteractable
    {
        [SerializeField] private Sprite _displayIcon;

        public string GetDisplayText()
            => "Open";

        public void Interact()
            => InventoryHandler.singleton.InventoryPanelsDisplayer.OpenWorkbenchPanel();

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
            => true;
    }
}
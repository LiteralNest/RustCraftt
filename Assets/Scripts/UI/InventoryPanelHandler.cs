using UnityEngine;

namespace UI
{
    public class InventoryPanelHandler : MonoBehaviour
    {
        private void OnEnable()
            => GlobalEventsContainer.InventoryClosed += DisablePanel;
        
        private void OnDisable()
            => GlobalEventsContainer.InventoryClosed -= DisablePanel;

        private void DisablePanel()
            => gameObject.SetActive(false);
    }
}
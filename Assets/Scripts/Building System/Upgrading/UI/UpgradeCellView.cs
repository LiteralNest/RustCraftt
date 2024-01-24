using Items_System.Items;
using UnityEngine;

namespace Building_System.Upgrading.UI
{
    public class UpgradeCellView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _active;
        [SerializeField] private GameObject _unActive;
        [SerializeField] private GameObject _upgradedPanel;
        [SerializeField] private Resource _targetResource;
        [SerializeField] private int _level;
        public Resource TargetResource => _targetResource;
        
        public void DisplayActive(int resourceCount, int currentLevel)
        {
            if (currentLevel >= _level)
            {
                _active.SetActive(false);
                _unActive.SetActive(false);
                _upgradedPanel.SetActive(true);
                return;
            }
            var itemCount = InventoryHandler.singleton.CharacterInventory.GetItemCount(_targetResource.Id);
            bool enoughMaterials = itemCount >= resourceCount;
            _active.SetActive(enoughMaterials);
            _unActive.SetActive(!enoughMaterials);
        }
    }
}
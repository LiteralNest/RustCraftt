using TMPro;
using UnityEngine;

namespace Alerts_System.Alerts.Item_Alert
{
    public abstract class ItemAlertDisplayer : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _itemTitle;
        [SerializeField] protected TMP_Text _itemCount;

        public virtual void Init(InventoryCell inventoryCell)
        {
            if (inventoryCell.Item == null) return;
            _itemTitle.text = inventoryCell.Item.Name;
        }

        public void Destroy()
            => Destroy(gameObject);
    }
}

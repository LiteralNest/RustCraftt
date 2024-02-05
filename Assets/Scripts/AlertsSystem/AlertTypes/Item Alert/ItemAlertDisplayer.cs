using TMPro;
using UnityEngine;

namespace AlertsSystem.AlertTypes.Item_Alert
{
    public abstract class ItemAlertDisplayer : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _itemTitle;
        [SerializeField] protected TMP_Text _itemCount;

        public virtual void Init(string itemName, int count)
        {
            _itemTitle.text = itemName;
        }

        public void Destroy()
            => Destroy(gameObject);
    }
}

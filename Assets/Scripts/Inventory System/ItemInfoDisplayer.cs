using Items_System.Items.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_System
{
    public class ItemInfoDisplayer : MonoBehaviour
    {
        public static ItemInfoDisplayer Singleton { get; private set; }

        [Header("UI")] [SerializeField] private GameObject _panel;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;

        private void Awake()
            => Singleton = this;

        public void DisplayItemInfo(Item item)
        {
            if (item == null)
            {
                _panel.SetActive(false);
                return;
            }

            _panel.SetActive(true);
            _icon.sprite = item.Icon;
            _titleText.text = item.Name;
            _descriptionText.text = item.Description;
        }
    }
}
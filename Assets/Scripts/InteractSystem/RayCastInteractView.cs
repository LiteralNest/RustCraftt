using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InteractSystem
{
    public class RayCastInteractView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _displayText;
        [SerializeField] private Image _targetImage;
        [SerializeField] private GameObject _displayPanel;
        [SerializeField] private Button _targetButton;

        public void DisplayData(IRaycastInteractable interactable)
        {
            _displayPanel.gameObject.SetActive(true);
            _displayText.text = interactable.GetDisplayText();
            _targetImage.sprite = interactable.GetIcon();
            _targetButton.onClick.RemoveAllListeners();
            _targetButton.onClick.AddListener(() => interactable.Interact());
        }

        public void ClosePanel()
        {
            _displayPanel.gameObject.SetActive(false);
            _displayText.text = "";
            _targetButton.onClick.RemoveAllListeners();
        }
    }
}
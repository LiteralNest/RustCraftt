using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RespawnSystem.SleepingBag
{
    [RequireComponent(typeof(SleepingBag))]
    public class SleepingBagNamer : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private GameObject _renamePanel;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        private SleepingBag _sleepingBag;

        private void Awake()
        {
            _sleepingBag = GetComponent<SleepingBag>();

            _confirmButton.onClick.AddListener(() =>
            {
                if (_inputField.text == "")
                    _sleepingBag.RenameServerRpc("Untitled");
                else
                    _sleepingBag.RenameServerRpc(_inputField.text);
                CloseUI();
            });

            _cancelButton.onClick.AddListener(CloseUI);
        }

        public void Open()
        {
            _renamePanel.SetActive(true);
            _inputField.text = _sleepingBag.Name.Value.ToString();
        }

        private void CloseUI()
            => _renamePanel.SetActive(false);
    }
}
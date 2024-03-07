using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cloud.DataBaseSystem.UserData.View
{
    public class RegisterPlayerView : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private GameObject _registerUI;
        [SerializeField] private Button _registerButton;
        [SerializeField] private Button _loginButton;
        [SerializeField] private TMP_InputField _inputName;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private TMP_Text _displayName;

        public void Init()
        {
            HandleInputFields(true);
            DisplayRegisterPanel(true);
            _inputName.onValueChanged.AddListener(OnInputFieldDataChanged);
        }

        public void DisplayRegisterPanel(bool value)
            => _registerUI.SetActive(value);

        public void HandleInputFields(bool value)
        {
            _inputName.interactable = value;
            _inputPassword.interactable = value;
            _loginButton.interactable = value;
            _registerButton.interactable = value;
            
        }

        private void OnInputFieldDataChanged(string inputText)
        {
            _registerButton.interactable = _inputName.text.Length > 0;
            _displayName.text = inputText;
        }
    }
}
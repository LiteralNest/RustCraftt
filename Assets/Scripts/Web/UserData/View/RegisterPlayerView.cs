using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Web.UserData.View
{
    public class RegisterPlayerView : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private GameObject _registerUI;
        [SerializeField] private Button _registerButton;
        [SerializeField] private TMP_InputField _inputName;
        [SerializeField] private TMP_Text _displayName;

        public void Init(UserJsonDataHandler handler)
        {
            _registerUI.gameObject.SetActive(true);
            _registerButton.onClick.AddListener(() => OnRegisterButtonClicked(handler));
            _inputName.onValueChanged.AddListener(OnInputFieldDataChanged);
        }

        private void OnInputFieldDataChanged(string inputText)
        {
            _registerButton.interactable = _inputName.text.Length > 0;
            _displayName.text = inputText;
        }

        private void OnRegisterButtonClicked(UserJsonDataHandler handler)
        {
            handler.SaveUserData(_inputName.text);
            _registerUI.gameObject.SetActive(false);
        }
    }
}
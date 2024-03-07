using Cloud.DataBaseSystem.UserData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cloud.DataBaseSystem.DataBaseServices.UserAuthorization
{
    public class UserAuthorizationView : MonoBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private UserJsonDataHandler _userJsonDataHandler;

        [Header("UI")] [SerializeField] private TMP_InputField _loginInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private Button _registerButton;
        [SerializeField] private Button _loginButton;

        private UserAuthorization _userAuthorization = new();

        private void Start()
        {
            _registerButton.onClick.AddListener(Register);
            _loginButton.onClick.AddListener(Login);
        }

        private async void Register()
        {
            var res = await _userAuthorization.RegisterPlayerAsync(_loginInput.text, _passwordInput.text);
            if (res == "true")
                await _userJsonDataHandler.SaveUserData(_loginInput.text);
            else
                _errorText.text = res;
        }

        private async void Login()
        {
            var res = await _userAuthorization.LoginPlayerAsync(_loginInput.text, _passwordInput.text);
            if (res == "true")
                await _userJsonDataHandler.SaveUserData(_loginInput.text);
            else
                _errorText.text = res;
        }
    }
}
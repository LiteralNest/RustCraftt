using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lock_System
{
    public class CodeLockUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _passwordText;
        [SerializeField] private Button[] _numberButtons;
        [SerializeField] private Button _clearButton;
        [SerializeField] private CodeLocker _codeLocker;
    
        private string _currentPassword = "";

        private void Start()
        {
            for (var i = 0; i < _numberButtons.Length; i++)
            {
                var number = i;
                _numberButtons[i].onClick.AddListener(() => OnNumberButtonPressed(number));
            }

            _clearButton.onClick.AddListener(OnClearButtonPressed);
        }
    
        private void OnNumberButtonPressed(int number)
        {
            if (_currentPassword.Length < 4)
            {
                _currentPassword += number.ToString();
                UpdatePasswordText();
            }
        }
    
        private void OnClearButtonPressed()
        {
            _currentPassword = "";
            UpdatePasswordText();
        }
    
        private void UpdatePasswordText()
        {
            _passwordText.text = new string('*', _currentPassword.Length);

            if (_currentPassword.Length == 4)
            {
                _codeLocker.OnEnteredPassword(_currentPassword);
                _currentPassword = "";
            }
        }
    
        // private void OnSubmitButtonPressed()
        // {
        //     if (currentPassword.Length == 4)
        //     {
        //         _codeLocker.OnPlayerApproach(UserDataHandler.singleton.UserData.Id);
        //         currentPassword = "";
        //     }
        // }
    }
}
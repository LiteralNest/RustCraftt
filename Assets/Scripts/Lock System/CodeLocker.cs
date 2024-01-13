using AuthorizationSystem;
using Multiplayer.CustomData;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Lock_System
{
    public class CodeLocker : Locker
    {
        [SerializeField] private GameObject _codeUI;
        [SerializeField] private NetworkVariable<AuthorizedUsersData> _authorizedIds = new();
        [field: SerializeField] public NetworkVariable<int> Password { get; private set; } = new NetworkVariable<int>();

        public override bool CanBeOpened(int value)
        {
            AuthorizationHelper helper = new AuthorizationHelper();
            if (!helper.IsAuthorized(value, _authorizedIds))
            {
                if (Password.Value != value)
                {
                    _codeUI.SetActive(true);
                    return false;
                }
            }
            return true;
        }

        public override void Init(int userId)
            => Open();

        public override bool IsLocked()
            => Password.Value != 0;

        [ServerRpc(RequireOwnership = false)]
        private void SetPasswordServerRpc(int password)
        {
            if (!IsServer) return;
            Password.Value = password;
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegistrateUserServerRpc(int userId)
        {
            if (!IsServer) return;
            var helper = new AuthorizationHelper();
            helper.Authorize(userId, _authorizedIds);
        }

        public override void Open()
            => _codeUI.SetActive(true);

        public void OnEnteredPassword(string enteredPassword)
        {
            if (int.TryParse(enteredPassword, out var parsedPassword))
            {
                if (Password.Value == 0)
                {
                    SetPasswordServerRpc(parsedPassword);
                    RegistrateUserServerRpc(UserDataHandler.Singleton.UserData.Id);
                    _codeUI.SetActive(false);
                }
                else
                {
                    if (parsedPassword == Password.Value)
                    {
                        RegistrateUserServerRpc(UserDataHandler.Singleton.UserData.Id);
                        _codeUI.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid password format!");
            }
        }
    }
}
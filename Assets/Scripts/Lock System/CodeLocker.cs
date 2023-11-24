using UnityEngine;
using Unity.Netcode;
using Web.User;

public class CodeLocker : NetworkBehaviour
{
    [SerializeField] private GameObject _codeUI;
    public int Password { get; private set; }

    private readonly NetworkList<int> _registeredPlayerIds = new();
    
    public void OnPlayerApproach(int playerId)
    {
        if (_registeredPlayerIds.Count == 0)
        {
            _codeUI.SetActive(true);
            int enteredPassword = GetEnteredPassword();
            RegistrateCode(playerId, enteredPassword);
        }
        else
        {
            if (!_registeredPlayerIds.Contains(playerId))
            {
                int enteredPassword = GetEnteredPassword();
                if (CanBeOpened(playerId, enteredPassword))
                {
                    RegistrateCode(playerId, enteredPassword);
                }
                else
                {
                    // Incorrect password logic
                }
            }
        }
    }
    
    public void RegistrateCode(int playerId, int password)
    {
        int realPlayerId = UserDataHandler.singleton.UserData.Id;

        if (realPlayerId == playerId)
        {
            _registeredPlayerIds.Add(playerId);
            Password = password;
            Debug.Log($"Code registered for player {playerId}: {password}");
        }
        else
        {
            Debug.Log($"Incorrect player ID: {playerId}");
        }
    }
    
    public bool CanBeOpened(int playerId, int enteredPassword)
    {
        return _registeredPlayerIds.Contains(playerId) && enteredPassword == Password;
    }
    
    private int GetEnteredPassword()
    {
        return Password; 
    }
    
    public void OnEnteredPassword(string enteredPassword)
    {
        if (int.TryParse(enteredPassword, out var parsedPassword))
        {
            Password = parsedPassword;
            _codeUI.SetActive(false);
            Debug.Log($"Entered password set: {Password}");
        }
        else
        {
            Debug.LogError("Invalid password format!");
        }
    }
}
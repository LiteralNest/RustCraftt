using UnityEngine;
using Unity.Netcode;
using System;
using Web.User;

public class CodeLocker : NetworkBehaviour
{
    public int Password { get; private set; }

    private readonly NetworkList<int> _registeredPlayerIds = new();

    // Method called when a player approaches the locker
    public void OnPlayerApproach(ushort playerId)
    {
        // If the list is empty, display the password input window
        if (_registeredPlayerIds.Count == 0)
        {
           
            int enteredPassword = GetEnteredPassword(); // Implement this method
            RegistrateCode(playerId, enteredPassword);
        }
        else
        {
            // If the player is not registered, ask for the password
            if (!_registeredPlayerIds.Contains(playerId))
            {
                // Display password input window logic goes here
                // Get the entered password and check if it's correct
                int enteredPassword = GetEnteredPassword();
                if (CanBeOpened(playerId, enteredPassword))
                {
                    // Player entered the correct password, register them
                    RegistrateCode(playerId, enteredPassword);
                }
                else
                {
                    // Incorrect password logic goes here
                }
            }
        }
    }

    // Method for registering the code and player ID
    public void RegistrateCode(ushort playerId, int password)
    {
        // Retrieve the player ID from UserDataHandler
        int realPlayerId = UserDataHandler.singleton.UserData.Id;

        if (realPlayerId == playerId)
        {
            _registeredPlayerIds.Add(playerId);
            Password = password;
            Debug.Log($"Code registered for player {playerId}: {password}");
        }
        else
        {
            // Handle incorrect player ID logic
            Debug.Log($"Incorrect player ID: {playerId}");
        }
    }

    // Method for checking if the locker can be opened by a player
    public bool CanBeOpened(int playerId, int enteredPassword)
    {
        return _registeredPlayerIds.Contains(playerId) && enteredPassword == Password;
    }

    // Method to simulate getting the entered password (replace with your actual logic)
    private int GetEnteredPassword()
    {
        // Replace this with your logic to get the entered password from the UI or other input method
        return 1234; // Replace with the actual password retrieval logic
    }
}

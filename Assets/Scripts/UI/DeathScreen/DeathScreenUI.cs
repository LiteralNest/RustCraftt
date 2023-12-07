using System.Linq;
using Multiplayer.PlayerSpawning;
using UnityEngine;

namespace UI.DeathScreen
{
    public class DeathScreenUI : MonoBehaviour
    {
        public void Respawn()
        {
            MainUiHandler.Singleton.DisplayDeathScreen(false);
            var playerKillers = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var playerKiller in playerKillers)
            { 
                playerKiller.Respawn(new Vector3(0, 1000000, 0));
            }
        }
        
        public void RespawnInCoordinates(Vector3 coordinates)
        {
            MainUiHandler.Singleton.DisplayDeathScreen(false);
            var playerKillers = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var playerKiller in playerKillers)
            { 
                playerKiller.Respawn(coordinates);
            }
        }
    }
}
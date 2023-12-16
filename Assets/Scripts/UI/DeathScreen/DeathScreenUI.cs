using System.Linq;
using Multiplayer.PlayerSpawning;
using UnityEngine;
using UI;

namespace UI.DeathScreen
{
    public class DeathScreenUI : MonoBehaviour
    {
        public static DeathScreenUI Singleton { get; private set; }

        private void Awake()
            => Singleton = this;

        public void Respawn()
        {
            MainUiHandler.Singleton.DisplayDeathScreen(false);
            var playerKillers = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var playerKiller in playerKillers)
            {
                if (!playerKiller.IsOwner) continue;
                playerKiller.Respawn(new Vector3(0, 1000000, 0));
            }
        }

        public void RespawnInCoordinates(Vector3 coordinates)
        {
            MainUiHandler.Singleton.DisplayDeathScreen(false);
            var playerKillers = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var playerKiller in playerKillers)
            {
                if (!playerKiller.IsOwner) continue;
                playerKiller.Respawn(coordinates);
            }
        }
    }
}
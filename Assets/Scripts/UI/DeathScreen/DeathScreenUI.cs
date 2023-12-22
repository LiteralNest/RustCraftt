using System.Linq;
using Map;
using Multiplayer.PlayerSpawning;
using UnityEngine;

namespace UI.DeathScreen
{
    public class DeathScreenUI : MonoBehaviour
    {
        public static DeathScreenUI Singleton { get; private set; }

        [SerializeField] private MapHandler _mapHandler;
        
        private void Awake()
            => Singleton = this;

        private void OnEnable()
            => _mapHandler.Open();

        private void OnDisable()
            => _mapHandler.Close();
        
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
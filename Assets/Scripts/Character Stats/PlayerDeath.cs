using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class PlayerDeath : NetworkBehaviour
    {
        private void Start()
        {
            GlobalEventsContainer.PlayerDied += OnPlayerDeath;
        }

        private void OnDestroy()
        {
            GlobalEventsContainer.PlayerDied -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            if (NetworkObject.IsLocalPlayer)
            {
                CharacterSpawnManager.Instance.OnPlayerDeath();
            }
        }
    }
}
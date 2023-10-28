using System;
using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class PlayerDeath : NetworkBehaviour
    {
        [SerializeField] private GameObject _deathScreen;
        
        private void Start()
        {
            GlobalEventsContainer.PlayerDied += OnPlayerDeath;
        }
        
        public override void OnDestroy()
        {
            GlobalEventsContainer.PlayerDied -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            var healt = CharacterStats.Singleton.Health;
            if (NetworkObject.IsLocalPlayer && healt >= 0)
            {
                _deathScreen.SetActive(true);
            }
        }
    }
}
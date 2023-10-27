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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CharacterStats.Singleton.TestDeathStat();
            }
        }
        public override void OnDestroy()
        {
            GlobalEventsContainer.PlayerDied -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            if (NetworkObject.IsLocalPlayer)
            {
                CharacterSpawnManager.Instance.OnPlayerDeath();
                _deathScreen.SetActive(true);
            }
        }
    }
}
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerDeath : NetworkBehaviour
    {
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private GameObject _deathScreen;
        [SerializeField] private GameObject _knockDownScreen;

        private void Start()
        {
            GlobalEventsContainer.PlayerDied += OnPlayerDeath;
            GlobalEventsContainer.PlayerKnockDowned += OnPlayerKnockDown;
        }
        
        public override void OnDestroy()
        {
            GlobalEventsContainer.PlayerDied -= OnPlayerDeath;
            GlobalEventsContainer.PlayerKnockDowned -= OnPlayerKnockDown;
        }

        
        private void OnPlayerKnockDown()
        {
            var health = _characterStats.Health;
            if (health is <= 5 and > 0)
            {
                _knockDownScreen.SetActive(true);
                CharacterStats.Singleton.MinusStat(CharacterStatType.Food,40);
                CharacterStats.Singleton.MinusStat(CharacterStatType.Water,40);
            }
        }
        private void OnPlayerDeath()
        {
            _knockDownScreen.SetActive(false);
            _deathScreen.SetActive(true);
            _deathScreen.SetActive(true);
            PlayerNetCode.Singleton.EnableColliders(false);
        }
    }
}
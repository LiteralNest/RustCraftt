using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class PlayerDeath : NetworkBehaviour
    {
        [SerializeField] private GameObject _deathScreen;
        [SerializeField] private GameObject _knockDownScreen;
        
        
        private void Start()
        {
            if (!NetworkObject.IsLocalPlayer) return;
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
            var health = CharacterStats.Singleton.Health;
            if (health is <= 5 and > 0)
            {
                _knockDownScreen.SetActive(true);
                CharacterStats.Singleton.MinusStat(CharacterStatType.Food,40);
                CharacterStats.Singleton.MinusStat(CharacterStatType.Water,40);
            }
        }
        private void OnPlayerDeath()
        {
            var healt = CharacterStats.Singleton.Health;
            if (healt <= 0)
            {
                _knockDownScreen.SetActive(false);
                _deathScreen.SetActive(true);
            }
        }
    }
}
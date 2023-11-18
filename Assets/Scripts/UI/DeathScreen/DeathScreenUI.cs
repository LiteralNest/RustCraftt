using System.Collections.Generic;
using Character_Stats;
using PlayerDeathSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.DeathScreen
{
    public class DeathScreenUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.InputSystem.PlayerInput _input;
        [SerializeField] private GameObject _gameHUD;
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _gameHUD.SetActive(false);
            _input.DeactivateInput();
        }

        public void Respawn()
        {
            var spawnPoints = PlayerRespawnManager.Instance.GetSpawnPoints();
            var selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            PlayerRespawnManager.Instance.OnSpawnPointSelected(selectedSpawnPoint.gameObject);
            CharacterStats.Singleton.ResetStatsToDefault();
            _input.ActivateInput();
            _gameHUD.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
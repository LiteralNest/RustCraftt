using System.Collections.Generic;
using Character_Stats;
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

        private List<Transform> _spawnPoints = new List<Transform>();

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _gameHUD.SetActive(false);
            _input.DeactivateInput();
            _spawnPoints = CharacterSpawnManager.Instance.GetSpawnPoints();
        }

        public void Respawn()
        {
            var selectedSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            CharacterSpawnManager.Instance.OnSpawnPointSelected(selectedSpawnPoint.gameObject);
            CharacterStats.Singleton.ResetStatsToDefault();
            _input.ActivateInput();
            _gameHUD.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
using System;
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
        [SerializeField] private Button _respawnButton;
        [SerializeField] private TMP_Dropdown _spawnPointDropdown;
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

            var spawnPointNames = new List<string>();
            foreach (var point in _spawnPoints)
            {
                spawnPointNames.Add(point.name);
            }
            _spawnPointDropdown.AddOptions(spawnPointNames);

            _respawnButton.onClick.AddListener(Respawn);
        }

        private void Respawn()
        {
            var selectedSpawnIndex = _spawnPointDropdown.value;
            var selectedSpawnPoint = _spawnPoints[selectedSpawnIndex];

            CharacterSpawnManager.Instance.OnSpawnPointSelected(selectedSpawnPoint.gameObject);
            CharacterStats.Singleton.ResetStatsToDefault();
            _input.ActivateInput();
            _gameHUD.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character_Stats
{
    
    public class CharacterSpawnManager : MonoBehaviour
    {
        [SerializeField] private int _numberOfSpawnPoints = 10;
        private readonly List<Transform> _spawnPoints = new();
    
        public static CharacterSpawnManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GenerateSpawnPoints();
            GlobalEventsContainer.PlayerDied += OnPlayerDeath;
        }

        private void GenerateSpawnPoints()
        {
            _spawnPoints.Clear();

            for (int i = 0; i < _numberOfSpawnPoints; i++)
            {
                Vector3 randomSpawnPoint = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                GameObject spawnPointObject = new GameObject("SpawnPoint" + i);
                spawnPointObject.transform.position = randomSpawnPoint;
                _spawnPoints.Add(spawnPointObject.transform);
            }
        }

        private void SpawnPlayerRandomly()
        {
            if (_spawnPoints.Count == 0)
            {
                Debug.LogError("No spawn points available!");
                return;
            }

            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            var playerTransform = PlayerNetCode.Singleton.transform;
            playerTransform.position = spawnPoint.position;

            GlobalEventsContainer.PlayerSpawned?.Invoke();
        }

        public void OnPlayerDeath()
        {
            SpawnPlayerRandomly();
            CharacterStats.Singleton.ResetStatsToDefault();
            
        }

        private void OnDisable()
        {
            GlobalEventsContainer.PlayerDied -= OnPlayerDeath;
        }
    }
}

        
        
    
 using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

 namespace Character_Stats
 {

     public class CharacterSpawnManager : MonoBehaviour
     {
         [SerializeField] private int _numberOfSpawnPoints = 10;
         [SerializeField] private GameObject _backpackPrefab;
         private readonly List<Transform> _spawnPoints = new();
         private Transform _lastPlayerTransform;
         
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

         public List<Transform> GetSpawnPoints()
         {
             return _spawnPoints;
         }
    
         //Spawn lootbox on the place of death
         public void OnPlayerDeath()
         {
             _lastPlayerTransform = PlayerNetCode.Singleton.transform;
             var playerTransform = _lastPlayerTransform;

             var cube = Instantiate(_backpackPrefab, playerTransform.position, playerTransform.rotation);

             var cubeRigidbody = cube.GetComponent<Rigidbody>();
             cubeRigidbody.isKinematic = true;
         }

         public void OnSpawnPointSelected(GameObject selectedSpawnPoint)
         {
             if (selectedSpawnPoint != null)
             {
                 RespawnPlayer(selectedSpawnPoint.transform);
                 CharacterStats.Singleton.ResetStatsToDefault();
             }
         }

         private void RespawnPlayer(Transform respawnPoint)
         {
             var playerTransform = PlayerNetCode.Singleton.transform;
             playerTransform.position = respawnPoint.position;

             GlobalEventsContainer.PlayerSpawned?.Invoke();
         }
     }
 }

        
        
    
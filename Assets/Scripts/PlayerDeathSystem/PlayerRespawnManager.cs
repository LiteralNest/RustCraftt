using System.Collections.Generic;
using Player_Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerDeathSystem
 {
     public class PlayerRespawnManager : MonoBehaviour
     {
         public static PlayerRespawnManager Instance { get; private set; }
         
         [SerializeField] private int _numberOfSpawnPoints = 10;
         [FormerlySerializedAs("_playersBackPackGenerator")] [SerializeField] private PlayerBackPackGenerator playerBackPackGenerator;

         private readonly List<Transform> _spawnPoints = new();
         private Transform _lastPlayerTransform;

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
                 spawnPointObject.transform.SetParent(transform);
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
             Time.timeScale = 1;
             _lastPlayerTransform = PlayerNetCode.Singleton.transform;
             var playerTransform = _lastPlayerTransform;
             playerBackPackGenerator.GenerateBackPackServerRpc(playerTransform.position, InventoryHandler.singleton.CharacterInventory.ItemsNetData.Value);
             InventoryHandler.singleton.CharacterInventory.ResetItemsServerRpc();
         }

         public void RespawnPlayer(Transform respawnPoint)
         {
             PlayerNetCode.Singleton.EnableColliders(true);
             var playerTransform = PlayerNetCode.Singleton.transform;
             playerTransform.position = respawnPoint.position;

             GlobalEventsContainer.PlayerSpawned?.Invoke();
         }

         public void OnSpawnPointSelected(GameObject selectedSpawnPoint)
         {
             if (selectedSpawnPoint != null)
             {
                 RespawnPlayer(selectedSpawnPoint.transform);
                 
             }
         }
     }
 }

        
        
    
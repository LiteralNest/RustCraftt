using System.Collections.Generic;
using UnityEngine;

namespace PlayerDeathSystem
 {
     public class PlayerSpawnManager : MonoBehaviour
     {
         public static PlayerSpawnManager Singleton { get; private set; }
         [SerializeField]  private List<Transform> _spawnPoints = new();

         private void Awake()
             => Singleton = this;

         public Vector3 GetRandomSpawnPoint()
             => _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
     }
 }

        
        
    
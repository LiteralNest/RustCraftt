using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

public class PlayerSpawnManager : NetworkManager
{
    [SerializeField] Transform[] spawnPoints; 
    [SerializeField] private NetworkObject _playerPrefab;
    
    public  void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Transform spawnPoint = GetRandomSpawnPoint(); 
        NetworkObject player = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation); 
       
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
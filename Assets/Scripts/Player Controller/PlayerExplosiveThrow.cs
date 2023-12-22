using Multiplayer.Multiplay_Instances;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

public class PlayerExplosiveThrow : NetworkBehaviour
{
    public static PlayerExplosiveThrow singleton { get; private set; }
    
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _throwForce = 10f;

    private int _currentId = -1;
    
    private void Awake()
        => singleton = this;

    public void SetCurrentId(int id)
        => _currentId = id;

    public void TryThrow()
    {
        if(_currentId == -1) return;
        SpawnAndThrowExplosiveServerRpc(_spawnPoint.position, _currentId);
        PlayerNetCode.Singleton.InHandObjectsContainer.SetDefaultHands();
        InventoryHandler.singleton.RemoveActiveSlotDisplayer();
    } 
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnAndThrowExplosiveServerRpc(Vector3 position, int id)
    {
        if(!IsServer) return;
        var obj = MultiplayObjectsPool.singleton.GetMultiplayInstanceIdById(id).gameObject;
        var explosive = Instantiate(obj, position, Quaternion.identity);
        explosive.GetComponent<NetworkObject>().Spawn(true);
        var rb = explosive.GetComponent<Rigidbody>();
        if (rb != null) rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
    }
}
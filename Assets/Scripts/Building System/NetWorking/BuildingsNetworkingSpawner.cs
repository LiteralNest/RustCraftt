using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BuildingObjectsPool))]
public class BuildingsNetworkingSpawner : NetworkBehaviour
{
   public static BuildingsNetworkingSpawner singleton { get; set; }

   [SerializeField] private BuildingObjectsPool _buildingObjectsPool;

   private void Awake()
   {
      singleton = this;
      _buildingObjectsPool = GetComponent<BuildingObjectsPool>();
   }
   
   
   [ServerRpc(RequireOwnership = false)]
   public void SpawnPrefServerRpc(int id, Vector3 pos, Quaternion rot)
   {
      if(!IsServer) return;
      var obj = Instantiate(_buildingObjectsPool.ObjectsPool[id], pos, rot);
      obj.DontDestroyWithOwner = true;
      obj.Spawn();
   }
}

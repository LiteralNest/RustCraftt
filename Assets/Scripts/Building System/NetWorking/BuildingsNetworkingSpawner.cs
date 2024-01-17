using Building_System.Blocks;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
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
      public void SpawnPrefServerRpc(int id, Vector3 pos, Quaternion rot, bool shouldPlaySound)
      {
         if(!IsServer) return;
         var obj = Instantiate(_buildingObjectsPool.GetObjectByPoolId(id), pos, rot);
         obj.DontDestroyWithOwner = true;
         obj.Spawn();
         if(shouldPlaySound)
            obj.GetComponent<BuildingBlock>().PlaySound();
      }
   }
}

using System.Collections.Generic;
using Building_System.Building.Placing_Objects;
using Cloud.CloudStorageSystem;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class PlacingObjectsPool : NetworkBehaviour
    {
        public static PlacingObjectsPool singleton { get; private set; }

        [SerializeField] private List<PlacingObject> _placingObjects = new List<PlacingObject>();
        
    
        private void Awake()
        {
            singleton = this;
        }

        private PlacingObject GetObjectById(int id)
        {
            foreach (var obj in _placingObjects)
                if (obj.TargetItem.Id == id)
                    return obj;
            Debug.LogError("Can't find object with id: " + id);
            return null;
        }

        public PlacingObject GetInstantiatedObjectOnServer(int id, Vector3 pos, Quaternion rot, int playerId = -1)
        {
            CloudSaveEventsContainer.OnStructureSpawned?.Invoke(id, pos, rot.eulerAngles);
            var obj = Instantiate(GetObjectById(id), pos, rot);
            obj.NetObject.Spawn();
            obj.SetOwnerId(playerId);
            obj.NetObject.DontDestroyWithOwner = true;
            return obj;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void InstantiateObjectServerRpc(int id, Vector3 pos, Quaternion rot, int playerId = -1)
        {
            if (!IsServer) return;
            GetInstantiatedObjectOnServer(id, pos, rot, playerId);
        }
    }
}
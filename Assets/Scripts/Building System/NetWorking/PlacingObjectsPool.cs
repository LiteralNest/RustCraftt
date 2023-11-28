using System.Collections.Generic;
using Building_System.Placing_Objects;
using Lock_System;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class PlacingObjectsPool : NetworkBehaviour
    {
        public static PlacingObjectsPool singleton { get; private set; }

        [SerializeField] private List<PlacingObject> _placingObjects = new List<PlacingObject>();

        public GameObject LastInstantiatedObj { get; private set; }
    
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

    
        [ServerRpc(RequireOwnership = false)]
        public void InstantiateObjectServerRpc(int id, Vector3 pos, Quaternion rot)
        {
            if (!IsServer) return;
            var obj = Instantiate(GetObjectById(id), pos, rot);
            obj.NetObject.Spawn();
            obj.NetObject.DontDestroyWithOwner = true;
        }
    
        [ServerRpc(RequireOwnership = false)]
        public void InstantiateObjectServerRpc(int id, Vector3 pos, Quaternion rot, int playerId)
        {
            if (!IsServer) return;
            var obj = Instantiate(GetObjectById(id), pos, rot);
            obj.GetComponent<KeyLocker>().RegistrateKey(playerId);
            obj.NetObject.DontDestroyWithOwner = true;
            obj.NetObject.Spawn();
        }
    }
}
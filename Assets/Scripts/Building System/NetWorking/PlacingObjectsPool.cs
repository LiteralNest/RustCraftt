using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

    [ServerRpc]
    public void InstantiateObjectServerRpc(int id, Vector3 pos, Quaternion rot)
    {
        if (!IsServer) return;
        var obj = Instantiate(GetObjectById(id), pos, rot);
        obj.NetworkObject.DontDestroyWithOwner = true;
        obj.NetworkObject.Spawn();
    }
}
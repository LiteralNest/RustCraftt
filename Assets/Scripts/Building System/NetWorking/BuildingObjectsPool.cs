using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingObjectsPool : MonoBehaviour
{
    [SerializeField] private List<BuildingStructure> _objectsPool = new List<BuildingStructure>();

    public NetworkObject GetObjectByPoolId(int id)
    {
        foreach (var obj in _objectsPool)
        {
            if (obj.Id == id)
                return obj.GetComponent<NetworkObject>();
        }
        Debug.LogError("Can't find object with id: " + id);
        return null;
    }
}

using System.Collections.Generic;
using Building_System.Building.Blocks;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class BuildingObjectsPool : MonoBehaviour
    {
        [SerializeField] private List<BuildingStructure> _objectsPool = new List<BuildingStructure>();

        public NetworkObject GetObjectByPoolId(int id)
        {
            foreach (var obj in _objectsPool)
            {
                if (obj.Id == id)
                    return obj.NetObject;
            }
            Debug.LogError("Can't find object with id: " + id);
            return null;
        }
    }
}

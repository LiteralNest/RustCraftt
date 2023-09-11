using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingObjectsPool : MonoBehaviour
{
    [field: SerializeField] public List<NetworkObject> ObjectsPool = new List<NetworkObject>();
}

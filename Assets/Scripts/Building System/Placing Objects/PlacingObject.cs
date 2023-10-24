using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlacingObject : MonoBehaviour
{
    [field: SerializeField] public NetworkObject NetworkObject { get; private set; }
    [field: SerializeField] public Item TargetItem { get; private set; }
    [field:SerializeField] public Vector3 ObjectSize { get; private set; }
}
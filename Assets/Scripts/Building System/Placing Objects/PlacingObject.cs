using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlacingObject : BuildingStructure
{
    [field: SerializeField] public Item TargetItem { get; private set; }
}
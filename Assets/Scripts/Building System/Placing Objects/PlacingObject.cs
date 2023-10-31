using Unity.Netcode;
using UnityEngine;

public class PlacingObject : BuildingStructure
{
    [field: SerializeField] public Item TargetItem { get; private set; }
}
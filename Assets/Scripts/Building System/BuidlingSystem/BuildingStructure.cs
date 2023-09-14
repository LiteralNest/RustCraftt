using System.Collections.Generic;
using UnityEngine;

public class BuildingStructure : MonoBehaviour
{
    [field:SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Vector3 StructureSize { get; private set; } = Vector3.one;
    [SerializeField] private List<BuildingBlock> _blocks = new List<BuildingBlock>(); 
}
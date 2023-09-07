using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingObjectSlot
{
    [field: SerializeField] public GameObject TargetObject { get; private set; }
    [field: SerializeField] public int Hp { get; private set; }
    [field: SerializeField] public List<InventoryCell> NeededCellsForPlace { get; private set; }
}

using UnityEngine;

[System.Serializable]
public class BuildingObjectSlot
{
    [field: SerializeField] public GameObject TargetObject { get; private set; }
    [field: SerializeField] public int Hp;
}

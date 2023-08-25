
using UnityEngine;

public class Item : ScriptableObject
{
    [field: SerializeField] public int ID;
    [field: SerializeField] public string Name;
    [field: SerializeField] public Sprite Icon;
    [field: SerializeField] public int StackCount = 1000;
}

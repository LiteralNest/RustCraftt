using UnityEngine;

public abstract class Item : ScriptableObject
{ 
    [field: SerializeField] public int Id;
    [field: SerializeField] public string Name;
    [field: SerializeField] public Sprite Icon;
    [TextArea]
    [field: SerializeField] public string Description;
    [field: SerializeField] public int TimeForCreating;
    [field: SerializeField] public int StackCount = 1000;

    public abstract void Click(InventoryHandler handler);
}

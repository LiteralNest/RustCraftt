using UnityEngine;

[System.Serializable]
public struct LootBoxSetItem
{
    [field:SerializeField] public Item Item { get; private set; }
    [field:SerializeField] public int Count { get; private set; }
}

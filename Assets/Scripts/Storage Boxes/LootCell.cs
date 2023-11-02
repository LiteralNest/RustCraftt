using UnityEngine;

[System.Serializable]
public class LootCell
{
    [field:SerializeField] public Item Item { get; set; }
    [field:SerializeField] public int MinimalCount { get; set; }
    [field:SerializeField] public int MaximalCount { get; set; }
}
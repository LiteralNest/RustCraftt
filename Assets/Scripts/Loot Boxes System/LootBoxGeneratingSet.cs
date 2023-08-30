using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LootBox Generating Set")]
public class LootBoxGeneratingSet : ScriptableObject
{
   [field:SerializeField] public List<LootBoxSetItem> Items = new List<LootBoxSetItem>();
}
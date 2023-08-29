using System.Collections.Generic;
using UnityEngine;

public class LootBoxGeneratingSet : ScriptableObject
{
   [field:SerializeField] public List<LootBoxSetItem> Items = new List<LootBoxSetItem>();
}
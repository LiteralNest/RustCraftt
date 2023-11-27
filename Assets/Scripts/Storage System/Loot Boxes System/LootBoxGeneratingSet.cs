using System.Collections.Generic;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
   [CreateAssetMenu(menuName = "ScriptableObjects/LootBox Generating Set")]
   public class LootBoxGeneratingSet : ScriptableObject
   {
      [field:SerializeField] public List<LootBoxSetItem> Items = new List<LootBoxSetItem>();
   }
}
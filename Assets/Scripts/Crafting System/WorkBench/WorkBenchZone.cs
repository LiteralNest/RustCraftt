using UnityEngine;

namespace Crafting_System.WorkBench
{
   public class WorkBenchZone : MonoBehaviour
   {
      [field: SerializeField] public int Level { get; private set; }
      [field:SerializeField] public WorkBench TargetWorkBench { get; private set; }
   }
}

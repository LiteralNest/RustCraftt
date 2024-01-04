using UnityEngine;

namespace Crafting_System.WorkBench
{
    public class CharacterWorkbenchesCatcher : MonoBehaviour
    {
        public int CurrentWorkBanchLevel { get; private set; }

        private void OnTriggerStay(Collider other)
        {
            var workbench = other.GetComponent<WorkBenchZone>();
            if(!workbench) return;
            CurrentWorkBanchLevel = workbench.Level;
        }

        private void OnTriggerExit(Collider other)
        {
            var workbench = other.GetComponent<WorkBenchZone>();
            if(!workbench) return;
            CurrentWorkBanchLevel = 0;
        }
    }
}
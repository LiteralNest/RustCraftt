using System;
using UnityEngine;

namespace Crafting_System
{
    public class CharacterWorkbanchesCatcher : MonoBehaviour
    {
        [field:SerializeField] public int CurrentWorkBanchLevel { get; private set; }

        private void OnTriggerStay(Collider other)
        {
            
        }
    }
}

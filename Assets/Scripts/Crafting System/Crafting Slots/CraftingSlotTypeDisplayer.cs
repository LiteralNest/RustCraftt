using UnityEngine;

namespace Crafting_System.Crafting_Slots
{
    public class CraftingSlotTypeDisplayer : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private GameObject _activeDisplay;

        public void DisplayActive(bool value)
            => _activeDisplay.SetActive(value);
    }
} 
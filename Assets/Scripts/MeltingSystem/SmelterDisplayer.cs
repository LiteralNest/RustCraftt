using Inventory_System.Slots_Displayer;
using UnityEngine;

namespace MeltingSystem
{
    public class SmelterDisplayer : MonoBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private SlotsDisplayer _slotsDisplayer;
    
        [Header("UI")] 
        [SerializeField] private GameObject _turnOnButton;
        [SerializeField] private GameObject _turnOffButton;

        public void DisplayButton(bool value)
        {
            _turnOnButton.SetActive(!value);
            _turnOffButton.SetActive(value);
        }
    }
}
using System.Collections.Generic;
using Items_System.Items;
using UnityEngine;

namespace PlayerDeathSystem.ArmorsSystem
{
    [System.Serializable]
    public struct CorpArmor
    {
        [SerializeField] private Armor _armor;
        [SerializeField] private List<CorpArmorSlot> _slots;
        [SerializeField] private List<GameObject> _activatedObjects;

        public int ArmorId => _armor.Id;

        public void Handle(bool value)
        {
            HandleObjects(value);
            ActivateSlots(value);
        }

        private void HandleObjects(bool value)
        {
            foreach (var obj in _activatedObjects)
                obj.SetActive(value);
        }

        private void ActivateSlots(bool value)
        {
            if (!value) return;
            foreach (var slot in _slots)
                slot.AssignMaterial();
        }
    }
}
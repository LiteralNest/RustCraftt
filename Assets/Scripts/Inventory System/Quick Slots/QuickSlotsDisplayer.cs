using System;
using System.Collections.Generic;
using Events;
using Inventory_System.Inventory_Slot_Displayers;
using UnityEngine;

namespace Inventory_System.Quick_Slots
{
    public class QuickSlotsDisplayer : MonoBehaviour
    {
        [SerializeField] private List<QuickSlotDisplayer> _quickSlots = new List<QuickSlotDisplayer>();

        private void OnEnable()
            => GlobalEventsContainer.OnActiveSlotReset += ResetActiveFons;

        private void OnDisable()
            => GlobalEventsContainer.OnActiveSlotReset -= ResetActiveFons;

        private void Start()
            => ClearSlots();

        private void ClearSlots()
        {
            foreach (var slot in _quickSlots)
                slot.Init();
        }

        private void ResetActiveFons()
        {
            foreach (var slot in _quickSlots)
                slot.HandleActive(false);
        }

        private ItemDisplayer GetGeneratedItemDisplayer(QuickSlotDisplayer quickSlotDisplayer,
            SlotDisplayer slotDisplayer)
        {
            var slotTransform = quickSlotDisplayer.transform;
            var instance =
                Instantiate(
                    InventorySlotDisplayerSelector.singleton.GetDisplayerByType(slotDisplayer.ItemDisplayer
                        .InventoryCell
                        .Item), slotTransform.position, slotTransform.rotation, slotTransform);
            instance.SetInventoryCell(slotDisplayer.ItemDisplayer.InventoryCell);
            return instance;
        }

        public void AssignSlots(List<SlotDisplayer> slotDisplayers)
        {
            ClearSlots();
            for (int i = 0; i < slotDisplayers.Count; i++)
            {
                if (slotDisplayers[i].ItemDisplayer == null) continue;
                var ItemDisplayer = GetGeneratedItemDisplayer(_quickSlots[i], slotDisplayers[i]);
                _quickSlots[i].ConnectedSlotDisplayer = slotDisplayers[i];
                _quickSlots[i].AssignItemDisplayer(ItemDisplayer);
            }
        }
    }
}
using System.Collections.Generic;
using Crafting_System.WorkBench;
using UnityEngine;

namespace Crafting_System.Crafting_Slots
{
    public class CraftingSlotsDisplayer : MonoBehaviour
    {
        [Header("Attached Scripts")]
        [SerializeField] private CraftingItemDataDisplayer _craftingItemDataDisplayer;
        [SerializeField] private CharacterWorkbenchesCatcher _characterWorkbenchesCatcher;

        [Header("Main Params")]
        [SerializeField] private List<CraftingSlotTypeDisplayer> _slotTypeDisplayers = new List<CraftingSlotTypeDisplayer>();
        
        [Header("UI")] [SerializeField] private CraftingSlotDisplayer _craftingSlotPrefab;
        [SerializeField] private Transform _placeForSlots;

        private void OnEnable()
            => DisplayFavourites(_slotTypeDisplayers[0]);

        private void ClearPlace(Transform place)
        {
            foreach (Transform child in place)
                Destroy(child.gameObject);
        }

        public void DisplaySlots(List<CraftingItem> inputSlots)
        {
            ClearPlace(_placeForSlots);
            foreach (var item in inputSlots)
                Instantiate(_craftingSlotPrefab, _placeForSlots)
                    .Init(item, _craftingItemDataDisplayer);
        }

        private List<CraftingItem> GetSlots<T>() where T : CraftingItem
        {
            List<CraftingItem> res = new List<CraftingItem>();
            foreach (var slot in ItemFinder.singleton.GetCraftingItems())
                if (slot is T && _characterWorkbenchesCatcher.CurrentWorkBanchLevel >= slot.NeededWorkBanch)
                    res.Add(slot as CraftingItem);
            return res;
        }

        private List<CraftingItem> GetFilteredListUsingIgnore<T>(List<CraftingItem> inputSlots) where T : CraftingItem
        {
            List<CraftingItem> res = new List<CraftingItem>();
            foreach (var slot in inputSlots)
                if (!(slot is T))
                    res.Add(slot as CraftingItem);
            return res;
        }
        
        private void DisplayActives(CraftingSlotTypeDisplayer displayer)
        {
            foreach (var slotTypeDisplayer in _slotTypeDisplayers)
                slotTypeDisplayer.DisplayActive(slotTypeDisplayer == displayer);
        }

        public void DisplayFavourites(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            ClearPlace(_placeForSlots);
        }

        public void DisplayBuildings(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            DisplaySlots(GetSlots<Building>());
        }

        public void DisplayTools(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            var slots = GetSlots<Tool>();
            slots = GetFilteredListUsingIgnore<Weapon>(slots);
            DisplaySlots(slots);
        }

        public void DisplayArmors(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            DisplaySlots(GetSlots<Armor>());
        }

        public void DisplayMedicine(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            DisplaySlots(GetSlots<Medicine>());
        }

        public void DisplayWeapons(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            DisplaySlots(GetSlots<Weapon>());
        }

        public void DisplayAmmo(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            DisplaySlots(GetSlots<Ammo>());
        }
    }
}
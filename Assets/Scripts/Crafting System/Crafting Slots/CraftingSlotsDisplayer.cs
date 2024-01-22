using System.Collections.Generic;
using Crafting_System.WorkBench;
using Inventory_System;
using Items_System.Items;
using Items_System.Items.Abstract;
using Items_System.Items.Weapon;
using TechTree;
using UnityEngine;
using Web.UserData;

namespace Crafting_System.Crafting_Slots
{
    public class CraftingSlotsDisplayer : MonoBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private CraftingItemDataDisplayer _craftingItemDataDisplayer;

        [SerializeField] private CharacterWorkbenchesCatcher _characterWorkbenchesCatcher;

        [Header("Main Params")] [SerializeField]
        private List<CraftingSlotTypeDisplayer> _slotTypeDisplayers = new List<CraftingSlotTypeDisplayer>();

        [Header("UI")] [SerializeField] private CraftingSlotDisplayer _craftingSlotPrefab;
        [SerializeField] private Transform _placeForSlots;

        private void OnEnable()
        {
            _craftingItemDataDisplayer.HandleInfoPanel(false);
            DisplayFavourites(_slotTypeDisplayers[0]);
        }
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

        private List<CraftingItem> GetConvertedResearchedList()
        {
            var res = new List<CraftingItem>();
            foreach (var itemId in
                     TechnologyManager.Singleton.GetResearchedTechs(UserDataHandler.Singleton.UserData.Id))
            {
                res.Add(ItemFinder.singleton.GetItemById(itemId) as CraftingItem);
            }

            return res;
        }

        private List<CraftingItem> GetCombinedList<T, Y>(List<T> firstList, List<Y> secondList) where T : CraftingItem where Y : CraftingItem
        {
            var res = new List<CraftingItem>();
            foreach (var item in firstList)
                res.Add(item);
            foreach (var item in secondList)
                res.Add(item);
            return res;
        }

        private List<CraftingItem> GetSlots<T>() where T : CraftingItem
        {
            List<CraftingItem> res = new List<CraftingItem>();
            var slots = GetCombinedList(ItemFinder.singleton.GetCraftingItems(), GetConvertedResearchedList());
            foreach (var slot in slots)
                if (slot is T && _characterWorkbenchesCatcher.CurrentWorkBanchLevel >= slot.NeededWorkBanch)
                    res.Add(slot);
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
            slots = GetFilteredListUsingIgnore<Explosion>(slots);
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
            var slots = GetSlots<Weapon>();
            var explosionSlots = GetSlots<Explosion>();
            slots.AddRange(explosionSlots);
            DisplaySlots(slots);
        }

        public void DisplayAmmo(CraftingSlotTypeDisplayer displayer)
        {
            DisplayActives(displayer);
            DisplaySlots(GetSlots<Ammo>());
        }
    }
}
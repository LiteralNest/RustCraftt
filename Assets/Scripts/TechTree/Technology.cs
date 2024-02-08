using System.Collections.Generic;
using Inventory_System;
using Items_System.Items.Abstract;
using UnityEngine;
using Web.UserData;

namespace TechTree
{
    public class Technology : MonoBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private Item _scrap;

        [SerializeField] private TechnologyUI _technologyUI;
        [field:SerializeField] public int Cost { get; private set; }

        [SerializeField] private List<Technology> _unlockingTech = new List<Technology>();
        [field: SerializeField] public Item Item { get; private set; }
        [SerializeField] private bool _isActive;
        public bool IsActive => _isActive;
        [field: SerializeField] public bool IsResearched { get; private set; }

        private void Awake()
            => _technologyUI.DisplayTech(this);

        private void UnlockTechs()
        {
            foreach (var tech in _unlockingTech)
                tech.Unlock();
        }

        public bool CanResearch()
        {
            if (!_isActive) return false;
            return InventoryHandler.singleton.CharacterInventory.GetItemCount(_scrap.Id) >= Cost;
        }
    
        private void Unlock()
        {
            _isActive = true;
            _technologyUI.UnlockTech();
        }

        public void Research()
        {
            if (IsResearched || !CanResearch()) return;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(_scrap.Id, Cost);
            IsResearched = true;
            _technologyUI.ResearchTech();
            UnlockTechs();
            TechnologyManager.Singleton.AddTechServerRpc(Item.Id, UserDataHandler.Singleton.UserData.Id);
        }
    }
}
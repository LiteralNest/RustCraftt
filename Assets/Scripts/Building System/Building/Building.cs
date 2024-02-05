using System.Collections.Generic;
using Inventory_System;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building
{
    public class Building : NetworkBehaviour
    {
        [Header("Building")]
        [SerializeField] private List<BuildingDamageSlot> _damageSlots;

        public float GetDamageAmount(int damageItemId)
        {
            var item = ItemFinder.singleton.GetItemById(damageItemId);
            foreach (var slot in _damageSlots)
                if (slot.DamageItem.Id == damageItemId)
                    return slot.DamageAmount;
            
            Debug.LogError("Item not found");
            return 0;
        }
    }
}
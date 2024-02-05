using System.Collections.Generic;
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
            foreach (var slot in _damageSlots)
                if (slot.DamageItem.Id == damageItemId)
                    return slot.DamageAmount;
            
            return 0;
        }

        public float GetDamageAmountByExplosive(int explosiveId, float distance, float radius)
        {
            foreach (var slot in _damageSlots)
            {
                if (slot.DamageItem.Id == explosiveId)
                    return  Mathf.Lerp(slot.DamageAmount, 0f, distance / radius);
            }

            return 0;
        }
    }
}
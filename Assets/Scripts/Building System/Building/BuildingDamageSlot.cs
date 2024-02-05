using Items_System.Items.Abstract;
using UnityEngine;

namespace Building_System.Building
{
    [System.Serializable]
    public struct BuildingDamageSlot
    {
        [SerializeField] private Item _damageItem;
        public Item DamageItem => _damageItem;
        [SerializeField] private float _damageAmount;
        public float DamageAmount => _damageAmount;
    }
}
using System.Collections.Generic;
using Armor_System.UI;
using Unity.Netcode;
using UnityEngine;

namespace Armor_System
{
    public class ArmorSlotsHandler : NetworkBehaviour
    {
        [SerializeField] private List<ArmorSlotDisplayer> _armorSlots = new List<ArmorSlotDisplayer>();
        [field: SerializeField] public NetworkVariable<int> HitResistValue { get; set; } = new(0);
        [field: SerializeField] public NetworkVariable<int> ExplosionResistValue { get; set; } = new(0);

        private void OnEnable()
            => ArmorSystemEventsContainer.ArmorSlotDataChanged += UpgradeData;

        private void OnDisable()
            => ArmorSystemEventsContainer.ArmorSlotDataChanged -= UpgradeData;

        private void UpgradeData()
        {
            AssignHitServerRpc(0);
            AssignExplosionServerRpc(0);
            foreach (var slot in _armorSlots)
            {
                var armor = slot.GetCurrentArmor();
                if (armor == null) continue;
                AssignHitServerRpc(HitResistValue.Value + armor.HitResistValue);
                AssignExplosionServerRpc(ExplosionResistValue.Value + armor.ExplosionResistValue);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AssignHitServerRpc(int value)
        {
            if (!IsServer) return;
            HitResistValue.Value = value;
        }

        [ServerRpc(RequireOwnership = false)]
        private void AssignExplosionServerRpc(int value)
        {
            if (!IsServer) return;
            ExplosionResistValue.Value = value;
        }
    }
}
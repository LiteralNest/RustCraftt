using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem.ArmorsSystem
{
    public class CorpesArmorsContainer : NetworkBehaviour
    {
        [SerializeField] private List<CorpArmor> _armors = new List<CorpArmor>();
        [SerializeField] private NetworkVariable<CopesArmorsNetData> _copesArmorsNetData = new();

        public override void OnNetworkSpawn()
        {
            DisplayIds(_copesArmorsNetData.Value.ArmorIds);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void AssignItemServerRpc(int itemId)
        {
            if (!IsServer) return;
            AssignItemClientRpc(itemId);
        }
        
        private void DisplayIds(int[] ids)
        {
            foreach (var id in ids)
            {
                foreach (var armor in _armors)
                {
                    if (armor.ArmorId != id) continue;
                    armor.Handle(true);
                    return;
                }
            }
        }

        [ClientRpc]
        private void AssignItemClientRpc(int itemId)
        {
            foreach (var armor in _armors)
            {
                if (armor.ArmorId != itemId) continue;
                armor.Handle(true);
                AddId(itemId);
                return;
            }
        }

        private void AddId(int value)
        {
            var ids = _copesArmorsNetData.Value.ArmorIds.ToList();
            ids.Add(value);
            _copesArmorsNetData.Value = new CopesArmorsNetData(ids.ToArray());
        }
    }
}
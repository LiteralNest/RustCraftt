using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class CharacterHpHandler : NetworkBehaviour, IDamagable
    {
        [SerializeField] private CharacterStats _characterStats;

        private ushort _defaultHp;

        private void Awake()
            => _defaultHp = (ushort)_characterStats.Health;

        public ushort GetHp()
            => (ushort)_characterStats.Health;

        public int GetMaxHp()
            => _defaultHp;

        public void GetDamage(int damage)
        {
            GetDamageServerRpc(GetComponent<NetworkObject>().NetworkObjectId, damage);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(ulong characterId, int damageAmount)
        {
            if(characterId != GetComponent<NetworkObject>().NetworkObjectId) return;
            _characterStats.MinusStat(CharacterStatType.Health, damageAmount);
        }
        
        public void Destroy(){}
    }
}
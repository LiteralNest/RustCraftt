using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class CharacterHpHandler : NetworkBehaviour, IDamagable
    {
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private CameraShake _cameraShake;

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

        public void Shake()
        {
            if(!_cameraShake) return;
            _cameraShake.StartShake(0.5f, 0.1f);
        }
    }
}
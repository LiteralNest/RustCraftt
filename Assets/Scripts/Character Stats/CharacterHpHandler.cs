using PlayerDeathSystem;
using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class CharacterHpHandler : NetworkBehaviour, IDamagable
    {
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private CameraShake _cameraShake;

        private NetworkVariable<int> _currentHp = new(50, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        private ushort _defaultHp;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _currentHp.Value = (int)_characterStats.Health;
            _currentHp.OnValueChanged += (int prevValue, int newValue) => DisplayDamage(newValue);
        }

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

        private void DisplayDamage(int damage)
        {
            if (_characterStats == null) return;
            _characterStats.MinusStat(CharacterStatType.Health, damage);
            if(IsOwner && _characterStats.Health <= 0)
                PlayerKnockDowner.Singleton.KnockDownServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(ulong characterId, int damageAmount)
        {
            if (!IsServer) return;
            _currentHp.Value -= damageAmount;

            //if(characterId != GetComponent<NetworkObject>().NetworkObjectId) return;
            // if (_characterStats == null) return;
            // _characterStats.MinusStat(CharacterStatType.Health, damageAmount);
        }

        public void Destroy()
        {
        }

        public void Shake()
        {
            if (!_cameraShake) return;
            _cameraShake.StartShake(0.5f, 0.1f);
        }
    }
}
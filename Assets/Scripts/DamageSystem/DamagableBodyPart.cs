using CharacterStatsSystem;
using FightSystem.Damage;
using Sound_System;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace DamageSystem
{
    public class DamagableBodyPart : NetworkBehaviour, IDamagable
    {
        [Header("Attached Scripts")]
        [SerializeField] private PlayerSoundsPlayer _playerSoundsPlayer; 

        [Header("Main Parameters")] [Range(0, 2)] [SerializeField]
        private float _gettingDamageKoef = 1;

        [SerializeField] private AudioClip _hitSound;

        private CharacterStats _characterStats;

        private void OnEnable()
        {
            CharacterStatsEventsContainer.OnCharacterStatsAssign += Init;
        }

        private void Init(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }

        public AudioClip GetPlayerDamageClip()
            => _hitSound;

        public int GetHp()
        {
            if (_characterStats == null) return 0;
            return _characterStats.Hp.Value;
        }

        public int GetMaxHp() => 100;

        public void GetDamageOnServer(int damage)
            => GetDamageClientRpc(damage);

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damage)
        {
            if (!IsServer) return;
            GetDamageOnServer(damage);
        }

        [ClientRpc]
        private void GetDamageClientRpc(int damage, int targetOwnerId)
        {
            if (UserDataHandler.Singleton.UserData.Id != targetOwnerId) return;
            if (_characterStats != null && _characterStats.Hp.Value > 0)
            {
                _playerSoundsPlayer.PlayHit(_hitSound);
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Health, damage);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damage, int targetOwnerId)
        {
            if (!IsServer) return;
            GetDamageClientRpc(damage, targetOwnerId);
        }

        public void GetDamageToServer(int damage)
            => GetDamageServerRpc(damage);

        [ClientRpc]
        private void GetDamageClientRpc(int damage)
        {
            if (!IsOwner) return;
            if (_characterStats != null && _characterStats.Hp.Value > 0)
            {
                _playerSoundsPlayer.PlayHit(_hitSound);
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Health, damage);
            }
        }

        public void Destroy()
        {
        }
    }
}
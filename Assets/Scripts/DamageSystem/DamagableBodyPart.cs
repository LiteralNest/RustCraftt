using CharacterStatsSystem;
using Cloud.DataBaseSystem.UserData;
using FightSystem.Damage;
using Player_Controller;
using Sound_System;
using Unity.Netcode;
using UnityEngine;

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

        public void GetDamageToServer(int damage)
            => GetDamageServerRpc(damage);

        [ClientRpc]
        private void GetDamageClientRpc(int damage)
        {
            if (!IsOwner) return;
            if (_characterStats != null && _characterStats.Hp.Value > 0)
            {
                _playerSoundsPlayer.PlayHit(_hitSound);
                var fixedDamage = damage * _gettingDamageKoef;
                var hitresist = PlayerNetCode.Singleton.ArmorSlotsHandler.HitResistValue.Value;
                if(hitresist > 0)
                    fixedDamage *= hitresist / 100;
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Health, (int)fixedDamage);
            }
        }

        public void Destroy()
        {
        }
    }
}
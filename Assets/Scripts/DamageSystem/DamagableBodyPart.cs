using ArmorSystem.Backend;
using CharacterStatsSystem;
using FightSystem.Damage;
using Sound_System;
using UnityEngine;

namespace DamageSystem
{
    public class DamagableBodyPart : MonoBehaviour, IDamagable
    {
        [Header("Attached Scripts")] [SerializeField]
        private PlayerSoundsPlayer _playerSoundsPlayer;

        [Header("Main Parameters")] [Range(0, 2)] [SerializeField]
        private float _gettingDamageKoef = 1;

        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private BodyPartType _partType = BodyPartType.None;

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
        {
            if (_characterStats.Hp.Value > 0)
            {
                _playerSoundsPlayer.PlayHit(_hitSound);
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Health, damage);
            }
        }

        public void Destroy()
        {
        }

        public void Shake()
        {
        }
    }
}
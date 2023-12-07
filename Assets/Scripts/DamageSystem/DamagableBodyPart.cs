using ArmorSystem.Backend;
using Character_Stats;
using Sound_System;
using UnityEngine;

namespace DamageSystem
{
    public class DamagableBodyPart : MonoBehaviour, IDamagable
    {
        [Header("Attached Scripts")] [SerializeField]
        private CharacterHpHandler _characterHpHandler;

        [Header("Main Parameters")] [Range(0, 2)] [SerializeField]
        private float _gettingDamageKoef = 1;

        [SerializeField] private BodyPartType _partType = BodyPartType.None;

        public ushort GetHp()
            => _characterHpHandler.Hp;

        public int GetMaxHp()
            => _characterHpHandler.DefaultHp;

        public void GetDamage(int damage)
        {
            if (_partType == BodyPartType.Head)
                PlayerSoundsPlayer.Singleton.PlayHeadShotSound();
            else
                PlayerSoundsPlayer.Singleton.PlayHitSound();
            _characterHpHandler.GetDamageServerRpc(
                (int)(damage * _gettingDamageKoef)); //Додати переввірку на резіст броні
        }

        public void Destroy()
        {
        }

        public void Shake()
            => _characterHpHandler.Shake();
    }
}
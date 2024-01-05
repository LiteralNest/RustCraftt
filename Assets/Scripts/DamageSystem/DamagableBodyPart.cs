using ArmorSystem.Backend;
using Character_Stats;
using Player_Controller;
using UnityEngine;

namespace DamageSystem
{
    public class DamagableBodyPart : MonoBehaviour, IDamagable
    {
        [Header("Attached Scripts")] [SerializeField]
        private CharacterHpHandler _characterHpHandler;

        [Header("Main Parameters")] [Range(0, 2)] [SerializeField]
        private float _gettingDamageKoef = 1;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private BodyPartType _partType = BodyPartType.None;

        public ushort GetHp()
        {
            if(_characterHpHandler.Hp <= 0)
                return 0;
            return (ushort)_characterHpHandler.Hp;
        }

        public int GetMaxHp() => 100;

        public void GetDamage(int damage, bool playSound = true)
        {
            if (_characterHpHandler.Hp >= 0)
            {
                PlayerNetCode.Singleton.PlayerSoundsPlayer.PlayHit(_hitSound);
                _characterHpHandler.GetDamageServerRpc(
                    (int)(damage * _gettingDamageKoef)); //Додати перевірку на резіст броні
            }
        }

        public void Destroy()
        {
        }

        public void Shake()
            => _characterHpHandler.Shake();
    }
}
using FightSystem.Damage;
using UnityEngine;

namespace Storage_System.Object_With_Loot
{
    public class LootObject : MonoBehaviour, IDamagable
    {
        private int _cachedHp;
        [SerializeField] private int _hp = 100;

        private void Start()
        {
            transform.tag = "DamagingItem";
            _cachedHp = _hp;
        }

        private void CheckHp()
        {
            if (_hp <= 0)
                Destroy(gameObject);
        }

        public int GetHp()
            => (ushort)_hp;

        public int GetMaxHp()
            => _cachedHp;

        public void GetDamage(int damage, bool playSound = true)
        {
            _hp -= damage;
            CheckHp();
        }

        public void Destroy()
        {
        
        }

        public void Shake()
        {
        
        }
    }
}
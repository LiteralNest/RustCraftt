using UnityEngine;

namespace Sound_System.FightSystem.Damage
{
    public interface IDamagable
    {
        public AudioClip GetPlayerDamageClip();
        public int GetHp();
        public int GetMaxHp();
        public void GetDamage(int damage);
        public void Destroy();
        public void Shake();
    }
}
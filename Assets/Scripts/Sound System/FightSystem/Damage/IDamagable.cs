using UnityEngine;

namespace Sound_System.FightSystem.Damage
{
    public interface IDamagable
    {
        public AudioClip GetPlayerDamageClip();
        public int GetHp();
        public int GetMaxHp();
        public void GetDamage(int damage, bool playSound = true);
        public void Destroy();
        public void Shake();
    }
}
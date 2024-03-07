using UnityEngine;

namespace FightSystem.Damage
{
    public interface IDamagable
    {
        public AudioClip GetPlayerDamageClip();
        public int GetHp();
        public int GetMaxHp();
        public void GetDamageOnServer(int damage);
        public void GetDamageToServer(int damage);
        public void Destroy();
    }
}
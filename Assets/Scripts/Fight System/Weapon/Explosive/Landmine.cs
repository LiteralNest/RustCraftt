using UnityEngine;

namespace Fight_System.Weapon.Explosive
{
    public class Landmine : BaseExplosive
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (_hasExploded) return;

            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("Mine");
                Explode();
                PlaySound();
                CharacterStats.Singleton.MinusStat(CharacterStatType.Health, _maxDamage);
            }
        }
    }
}
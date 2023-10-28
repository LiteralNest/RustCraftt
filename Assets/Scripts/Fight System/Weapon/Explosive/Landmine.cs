using UnityEngine;

public class Landmine : BaseExplosive
{
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasExploded) return;

        if (collision.collider.CompareTag("Player"))
        {
            Explode();
            CharacterStats.Singleton.MinusStat(CharacterStatType.Health, _maxDamage);
        }
    }
}
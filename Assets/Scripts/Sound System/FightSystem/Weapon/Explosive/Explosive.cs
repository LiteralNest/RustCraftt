using UnityEngine;

namespace FightSystem.Weapon.Explosive
{
    public class Explosive : BaseExplosive
    {
        [SerializeField] private float _timeToExplode = 3f;
        protected float CountdownTimer;
        protected bool Exploded = false;

        protected override void Start()
        {
            base.Start();
            InitTimer();
        }

        private void Update()
        {
            if (_hasExploded) return;
            CountdownTimer -= Time.deltaTime;

            if (CountdownTimer <= 0f && !Exploded)
            {
                ExplodeServerRpc();
                Exploded = true;
            }
        }

        protected void InitTimer()
            => CountdownTimer = _timeToExplode;
    }
}
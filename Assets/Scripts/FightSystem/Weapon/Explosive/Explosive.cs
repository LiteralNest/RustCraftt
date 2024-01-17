using UnityEngine;

namespace FightSystem.Weapon.Explosive
{
    public class Explosive : BaseExplosive
    {
        [SerializeField] private float _timeToExplode = 3f;
        private float _countdownTimer;
        private bool _exploded = false;

        protected override void Start()
        {
            base.Start();
            _countdownTimer = _timeToExplode;
        }

        private void Update()
        {
            if (_hasExploded) return;
            _countdownTimer -= Time.deltaTime;

            if (_countdownTimer <= 0f && !_exploded)
            {
                ExplodeServerRpc();
                _exploded = true;
            }
        }
    }
}
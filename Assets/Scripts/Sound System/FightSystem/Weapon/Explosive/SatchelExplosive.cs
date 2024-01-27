using UnityEngine;

namespace FightSystem.Weapon.Explosive
{
    public class SatchelExplosive : Explosive
    {
        [Tooltip("%")] [Range(0, 100)] [SerializeField]
        private int _explodeChance = 80;

        private bool _turnedOff;
        public bool TurnedOff => _turnedOff;

        protected override void Start()
        {
            base.Start();
            transform.tag = "Satchel";
        }

        private void Update()
        {
            if (_hasExploded) return;
            CountdownTimer -= Time.deltaTime;

            if (CountdownTimer <= 0f && !Exploded)
                TryExplode();
        }

        public void TurnOn()
        {
            _turnedOff = false;
            InitTimer();
        }

        private void TryExplode()
        {
            if(_turnedOff) return;
            var random = Random.Range(0, 100);
            if (random > _explodeChance)
            {
                _turnedOff = true;
                return;
            }

            if(_turnedOff) return;
            ExplodeServerRpc();
            Exploded = true;
        }
    }
}
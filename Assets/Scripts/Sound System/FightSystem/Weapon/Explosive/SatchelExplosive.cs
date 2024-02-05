using InteractSystem;
using UnityEngine;

namespace Sound_System.FightSystem.Weapon.Explosive
{
    public class SatchelExplosive : global::FightSystem.Weapon.Explosive.Explosive, IRaycastInteractable
    {
        [Tooltip("%")] [Range(0, 100)] [SerializeField]
        private int _explodeChance = 80;

        private bool _turnedOff;

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

        #region IRayCastInteractable

        public string GetDisplayText()
            => "Fire";

        public void Interact()
            => TurnOn();

        public bool CanInteract()
            => _turnedOff;

        #endregion
    }
}
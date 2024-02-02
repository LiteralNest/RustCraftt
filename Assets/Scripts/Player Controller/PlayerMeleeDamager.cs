using System.Collections;
using FightSystem.Damage;
using Sound_System.FightSystem.Damage;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerMeleeDamager : MonoBehaviour
    {
        [SerializeField] private float _damageRange = 1f;
        [SerializeField] private LayerMask _damageLayer;

        private bool _canDamage = true;

        private void Start()
            => _canDamage = true;

        public bool TryDamage(int damageAmount, float coolDownTime)
        {
            if (!_canDamage) return false;
            StartCoroutine(ResetCanDamageRoutine(coolDownTime));
            var cameraTransform = Camera.main.transform;
            var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out var hit, _damageRange, _damageLayer))
            {
                var damagable = hit.collider.GetComponent<IDamagable>();
                if (damagable == null) return false;
                damagable.GetDamage(damageAmount);
                return true;
            }

            return false;
        }

        private IEnumerator ResetCanDamageRoutine(float coolDownTime)
        {
            _canDamage = false;
            yield return new WaitForSeconds(coolDownTime);
            _canDamage = true;
        }
    }
}
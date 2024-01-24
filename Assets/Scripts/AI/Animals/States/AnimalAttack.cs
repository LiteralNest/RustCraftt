using System.Collections;
using FightSystem.Damage;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI.Animals.States
{
    public class AnimalAttack : AnimalState
    {
        [SerializeField] private float _attackRange = 1f;
        [SerializeField] private int _damagingForce = 10;
        [SerializeField] private float _damageCoolDown = 1f;

        private bool _canDamage = true;
        private readonly string _run = "Run";
        private readonly string _attack = "Attack";
        public override void Init(AnimalController controller)
        {
            base.Init(controller);
            StartCoroutine(AttackTarget());
        }

        private void TryAttack(Transform target)
        {
            var damagable = target.GetComponent<IDamagable>();
            if(damagable == null) return;
            damagable.GetDamage(_damagingForce);
            StartCoroutine(CoolDown());
        }
        
        private IEnumerator CoolDown()
        {
            _canDamage = false;
            yield return new WaitForSeconds(_damageCoolDown);
            _canDamage = true;
        }
        
        private IEnumerator AttackTarget()
        {
            var nearestTarget = _controller.GetNearestObject();
            if (nearestTarget == null)
            {
                _controller.SetIdleState();
                yield break;
            }

            while (true)
            {
                _controller.NavMeshAgent.SetDestination(nearestTarget.position);
                _controller.SetAnimState(_run);
                
                if(_controller.GetDistanceTo(nearestTarget.position) > _controller.InteractingRange.y)
                {
                    _controller.SetIdleState();
                    break;
                }

                if(!_canDamage) yield return null;
                if (_controller.GetDistanceTo(nearestTarget.position) <= _attackRange)
                {
                    TryAttack(nearestTarget);
                    _controller.SetAnimState(_attack);
                }
                yield return null;
            }
            _controller.SetIdleState();
        }
    }
}

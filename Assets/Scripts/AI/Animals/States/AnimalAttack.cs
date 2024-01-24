using System.Collections;
using AI.Animals.Animators;
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

        public override void Init(AnimalController controller, AnimalAnimator animalAnimator)
        {
            base.Init(controller, animalAnimator);
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
            var nearestTarget = Controller.GetNearestObject();
            if (nearestTarget == null)
            {
                Controller.SetIdleState();
                yield break;
            }
            AnimalAnimator.SetRun();
            while (true)
            {
                Controller.NavMeshAgent.SetDestination(nearestTarget.position);
                

                if(Controller.GetDistanceTo(nearestTarget.position) > Controller.InteractingRange.y)
                {
                    Controller.SetIdleState();
                    break;
                }

                if(!_canDamage) yield return null;
                if (Controller.GetDistanceTo(nearestTarget.position) <= _attackRange)
                {
                    TryAttack(nearestTarget);
                    AnimalAnimator.SetAttack();
                }
                yield return null;
            }
            AnimalAnimator.SetIdle();
            Controller.SetIdleState();
        }
    }
}

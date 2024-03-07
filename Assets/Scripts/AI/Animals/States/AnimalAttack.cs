using System.Collections;
using AI.Animals.Animators;
using FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

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
            if(!NetworkManager.Singleton.IsServer) return;
            Debug.Log("Damaging");
            var damagable = target.GetComponent<IDamagable>();
            if(damagable == null) return;
            damagable.GetDamageOnServer(_damagingForce);
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
                if (nearestTarget == null)
                {
                    AnimalAnimator.SetIdle();
                    Controller.SetIdleState();
                }
                Controller.NavMeshAgent.speed = Controller.RunSpeed;
                Controller.NavMeshAgent.SetDestination(nearestTarget.position);
                

                if(Controller.GetDistanceTo(nearestTarget.position) > Controller.InteractingRange.y)
                {
                    Controller.SetIdleState();
                    break;
                }

                if (!_canDamage)
                {
                    yield return null;
                    continue;
                }
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

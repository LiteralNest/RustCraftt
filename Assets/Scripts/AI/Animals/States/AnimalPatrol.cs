using System.Collections;
using AI.Animals.Animators;
using UnityEngine;

namespace AI.Animals.States
{
    public class AnimalPatrol : AnimalState
    {
        [SerializeField] private RandomCirclePointGetter _pointGetter;
        private Vector3 _currentMovingPoint;

        public override void Init(AnimalController controller, AnimalAnimator animalAnimator)
        {
            base.Init(controller, animalAnimator);
            FindNextPatrolPoint();
        }

        private void FindNextPatrolPoint()
        {
            _currentMovingPoint = _pointGetter.GetRandomPointInCircle();
            StartCoroutine(MoveToPoint());
        }

        
        
        private IEnumerator MoveToPoint()
        {
            Controller.NavMeshAgent.speed = Controller.WalkSpeed;
            Controller.NavMeshAgent.SetDestination(_currentMovingPoint);
            AnimalAnimator.SetWalk();
            
            while (!EnoughDistance(_currentMovingPoint))
            {
                if(_shouldStop) yield break;
                yield return null;
            }
            
            AnimalAnimator.SetIdle();
            Controller.SetIdleState();
        }

        private bool EnoughDistance(Vector3 pos)
            => Controller.GetDistanceTo(pos) < 0.5f;
    }
}
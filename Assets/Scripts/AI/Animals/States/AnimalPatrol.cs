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

        private void AssignNextPatrolPoint()
        {
            int breakCounter = 100;
            var navMeshAgent = Controller.NavMeshAgent;
            navMeshAgent.speed = Controller.WalkSpeed;
            while (true)
            {
                breakCounter--;
                if (breakCounter <= 0)
                {
                    Controller.SetIdleState();
                    break;
                }
                
                _currentMovingPoint = _pointGetter.GetRandomPointInCircle();
                navMeshAgent.SetDestination(_currentMovingPoint);
                if(navMeshAgent.pathEndPosition != _currentMovingPoint) continue;
                AnimalAnimator.SetWalk();
                break;
            }
          
        }
        private IEnumerator MoveToPoint()
        {
            AssignNextPatrolPoint();
            
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
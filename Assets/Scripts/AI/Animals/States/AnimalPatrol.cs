using System.Collections;
using AI.Animals.Animators;
using UnityEngine;
using UnityEngine.AI;

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
            var randomPoint = _pointGetter.GetRandomPointInCircle();
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas);
            _currentMovingPoint = hit.position;
            Controller.NavMeshAgent.SetDestination(_currentMovingPoint);
            AnimalAnimator.SetWalk();
        }
        
        // private void AssignNextPatrolPoint()
        // {
        //     int breakCounter = 1000;
        //     var navMeshAgent = Controller.NavMeshAgent;
        //     navMeshAgent.speed = Controller.WalkSpeed;
        //     while (true)
        //     {
        //         breakCounter--;
        //         if (breakCounter <= 0)
        //         {
        //             Controller.SetIdleState();
        //             break;
        //         }
        //         
        //         _currentMovingPoint = _pointGetter.GetRandomPointInCircle();
        //         navMeshAgent.SetDestination(_currentMovingPoint);
        //         if (navMeshAgent.pathEndPosition != _currentMovingPoint)
        //         {
        //             navMeshAgent.isStopped = true;
        //             continue;
        //         }
        //         AnimalAnimator.SetWalk();
        //         break;
        //     }
        //   
        // }
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
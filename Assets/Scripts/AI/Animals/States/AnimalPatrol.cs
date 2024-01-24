using System.Collections;
using UnityEngine;

namespace AI.Animals.States
{
    public class AnimalPatrol : AnimalState
    {
        [SerializeField] private RandomCirclePointGetter _pointGetter;
        private Vector3 _currentMovingPoint;
        private readonly string _walk = "Walk";
        public override void Init(AnimalController controller)
        {
            base.Init(controller);
            FindNextPatrolPoint();
        }

        private void FindNextPatrolPoint()
        {
            _currentMovingPoint = _pointGetter.GetRandomPointInCircle();
            StartCoroutine(MoveToPoint());
        }

        
        
        private IEnumerator MoveToPoint()
        {
            _controller.NavMeshAgent.SetDestination(_currentMovingPoint);
            _controller.SetAnimState(_walk);
            
            while (!EnoughDistance(_currentMovingPoint))
            {
                if(_shouldStop) yield break;
                yield return null;
            }
            _controller.SetIdleState();
        }

        private bool EnoughDistance(Vector3 pos)
            => _controller.GetDistanceTo(pos) < 0.5f;
    }
}
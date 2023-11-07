using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Animals.States
{
    public class AnimalPatrol : AnimalState
    {
        [SerializeField] private List<Transform> _patrolPoints;

        private Transform _currentMovingPoint;
        
        public override void Init(AnimalController controller)
        {
            base.Init(controller);
            FindNextPatrolPoint();
        }

        private void FindNextPatrolPoint()
        {
            var rand = Random.Range(0, _patrolPoints.Count);
            if (_patrolPoints != null && _currentMovingPoint == _patrolPoints[rand])
            {
                FindNextPatrolPoint();
                return;
            }

            _currentMovingPoint = _patrolPoints[rand];
            StartCoroutine(MoveToPoint());
        }

        
        
        private IEnumerator MoveToPoint()
        {
            _controller.NavMeshAgent.SetDestination(_currentMovingPoint.position);
            while (!EnoughDistance(_currentMovingPoint.position))
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
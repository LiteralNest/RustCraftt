using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals.AI.States
{
    public class AnimalRunningAway : AnimalState
    {
        public override void Init(AnimalController controller)
        {
           base.Init(controller);
           StartCoroutine(RunAway());
        }

        private IEnumerator RunAway()
        {
            Transform nearestObject = null;
            var cachedNearestObject = nearestObject;
            while (true)
            {
                nearestObject = _controller.GetNearestObjectToRunFrom();
                if(nearestObject == null)
                    break;
                if(nearestObject == cachedNearestObject)
                    yield return null;
                cachedNearestObject = nearestObject;
                Vector3 runDirection = transform.position - nearestObject.position;
                runDirection.Normalize();
                Vector3 destination = transform.position + runDirection * 10.0f;
                _controller.NavMeshAgent.SetDestination(destination);
                yield return null;
            }
            _controller.SetIdleState();
        }
    }
}
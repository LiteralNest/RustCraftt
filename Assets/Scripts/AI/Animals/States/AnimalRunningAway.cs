using System.Collections;
using AI.Animals.Animators;
using UnityEngine;

namespace AI.Animals.States
{
    public class AnimalRunningAway : AnimalState
    {
        public override void Init(AnimalController controller, AnimalAnimator animalAnimator)
        {
           base.Init(controller, animalAnimator);
           StartCoroutine(RunAway());
        }

        private IEnumerator RunAway()
        {
            Transform nearestObject = null;
            var cachedNearestObject = nearestObject;
                AnimalAnimator.SetRun();
            while (true)
            {
                nearestObject = Controller.GetNearestObject();
                if(nearestObject == null)
                    break;
                if(nearestObject == cachedNearestObject)
                    yield return null;
                cachedNearestObject = nearestObject;
                Vector3 runDirection = transform.position - nearestObject.position;
                runDirection.Normalize();
                Vector3 destination = transform.position + runDirection * 10.0f;
                Controller.NavMeshAgent.speed = Controller.RunSpeed;
                Controller.NavMeshAgent.SetDestination(destination);
                yield return null;
            }

            AnimalAnimator.SetIdle();
            Controller.SetIdleState();
        }
    }
}
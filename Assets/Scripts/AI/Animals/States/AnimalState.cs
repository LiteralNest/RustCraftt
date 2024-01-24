using AI.Animals.Animators;
using UnityEngine;

namespace AI.Animals.States
{
    public abstract class AnimalState : MonoBehaviour
    {
        protected AnimalAnimator AnimalAnimator;
        protected AnimalController Controller;
        protected bool _shouldStop;

        public virtual void Init(AnimalController controller, AnimalAnimator animalAnimator)
        {
            AnimalAnimator = animalAnimator;
            _shouldStop = false;
            Controller = controller;
        }

        public void Stop()
            => _shouldStop = true;
    }
}
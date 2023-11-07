using UnityEngine;

namespace AI.Animals.States
{
    public abstract class AnimalState : MonoBehaviour
    {
        protected AnimalController _controller;
        protected bool _shouldStop;

        public virtual void Init(AnimalController controller)
        {
            _shouldStop = false;
            _controller = controller;
        }

        public void Stop()
            => _shouldStop = true;
    }
}
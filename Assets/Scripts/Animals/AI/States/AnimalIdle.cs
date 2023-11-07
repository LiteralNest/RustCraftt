using System.Collections;
using UnityEngine;

namespace Animals.AI.States
{
    public class AnimalIdle : AnimalState
    {
        [SerializeField] private AnimalState _nextAnimalState;
        [SerializeField] private Vector2 _waitingTime;
        public override void Init(AnimalController controller)
        {
            base.Init(controller);
            StartCoroutine(WaitRandTime());
        }

        public IEnumerator WaitRandTime()
        {
            var waitingTime = Random.Range(_waitingTime.x, _waitingTime.y + 1);
            while (waitingTime > 0)
            {
                if(_shouldStop)
                    yield break;
                waitingTime -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            _controller.SetState(_nextAnimalState);
        } 
    }
}

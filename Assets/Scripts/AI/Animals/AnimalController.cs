using System.Collections.Generic;
using AI.Animals.Animators;
using AI.Animals.States;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Animals
{
    public abstract class AnimalController : MonoBehaviour
    {
        [Header("Attached Components")] 
        [SerializeField] private AnimalAnimator _animalAnimator;
        [SerializeField] protected List<AIPerception> _aIPerceptions = new List<AIPerception>();

        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        [Header("Main Params")] [SerializeField]
        private Vector2 _interactingRange = new Vector2(4, 10);

        [Header("States")] [SerializeField] protected AnimalState _idleState;
        [SerializeField] private AnimalState _playerInteractionState;

        public Vector2 InteractingRange => _interactingRange;

        private AnimalState _currentState;
        protected List<Transform> ObjectsToInteract { get; private set; } = new List<Transform>();

        private void Update()
        {
            if (_currentState == _playerInteractionState) return;
            var nearestObject = GetNearestObject();
            if (nearestObject == null) return;
            if (GetDistanceTo(nearestObject.position) < InteractingRange.x)
                SetState(_playerInteractionState);
        }

        private void Start()
            => SetState(_idleState);

        public virtual void RefreshList()
        {
            ObjectsToInteract.Clear();
        }

        public Transform GetNearestObject()
        {
            Transform nearestObject = null;
            float nearestDistance = Mathf.Infinity;

            foreach (var obj in ObjectsToInteract)
            {
                var objTransform = obj.transform;
                float distance = Vector3.Distance(transform.position, objTransform.position);

                if (distance > InteractingRange.y) continue;

                if (distance < nearestDistance)
                {
                    nearestObject = objTransform;
                    nearestDistance = distance;
                }
            }

            return nearestObject;
        }

        public void SetState(AnimalState state)
        {
            if (_currentState != null)
                _currentState.Stop();
            _currentState = state;
            _currentState.Init(this, _animalAnimator);
        }

        public void SetIdleState()
            => SetState(_idleState);

        public float GetDistanceTo(Vector3 pos)
        {
            pos.y = 0;
            Vector3 aiPos = transform.position;
            aiPos.y = 0;
            return Vector3.Distance(pos, aiPos);
        }
    }
}
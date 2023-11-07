using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animals.AI.States;
using UnityEngine;
using UnityEngine.AI;

namespace Animals.AI
{
    public class AnimalController : MonoBehaviour
    {
        [Header("Main Params")] 
        [SerializeField] private float _playersListRefreshingTime = 5f;
        [SerializeField] private float _minimalDistanceToRunAway = 4f;
        [SerializeField] private float _minimalDistanceToStopRunningAway = 10f;

        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        [Header("States")] [SerializeField] private AnimalState _idleState;
        [SerializeField] private AnimalState _patrolState;
        [SerializeField] private AnimalState _animalRunningAwayState;

        public List<PlayerNetCode> ObjectsToRunFrom { get; private set; } = new List<PlayerNetCode>();

        private AnimalState _currentState;

        private void InitList()
        {
            ObjectsToRunFrom = FindObjectsByType<PlayerNetCode>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .ToList();
        }
        
        private void Start()
        {
            StartCoroutine(RefreshListRoutine());
            SetState(_idleState);
        }

        private void Update()
        {
            if(_currentState == _animalRunningAwayState) return;
            var nearestObject = GetNearestObjectToRunFrom();
            if(nearestObject == null) return;
            if(GetDistanceTo(nearestObject.position) < _minimalDistanceToRunAway)
                SetState(_animalRunningAwayState);
        }

        private IEnumerator RefreshListRoutine()
        {
            while (true)
            {
                InitList();
                yield return new WaitForSeconds(_playersListRefreshingTime);
            }
        }

        public void SetIdleState()
            => SetState(_idleState);

        public Transform GetNearestObjectToRunFrom()
        {
            Transform nearestObject = null;
            float nearestDistance = Mathf.Infinity;

            foreach (var obj in ObjectsToRunFrom)
            {
                var objTransform = obj.transform;
                float distance = Vector3.Distance(transform.position, objTransform.position);

                if(distance > _minimalDistanceToStopRunningAway) continue;
                
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
            if(_currentState != null)
                _currentState.Stop();
            _currentState = state;
            _currentState.Init(this);
        }

        public float GetDistanceTo(Vector3 pos)
        {
            pos.y = 0;
            Vector3 aiPos = transform.position;
            aiPos.y = 0;
            return Vector3.Distance(pos, aiPos);
        }
    }
}
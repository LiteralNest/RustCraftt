using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DayTime
{
    public class SkyBoxHandler : NetworkBehaviour
    {
        [Header("Attached Components")] 
        [SerializeField] private SkyBoxView _skyBoxView;
        [SerializeField] private TimeHandler _dayTimeHandler;
        [SerializeField] private TimeHandler _nightTimeHandler;

        [Header("Main Value")] 
        [SerializeField] private float _timeScale;
        [SerializeField] private float _rotationScale = 0.01f;

        private Coroutine _rotationCoroutine;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsServer) return;
            StartCoroutine(RunDayTimeRoutine()); 
        }

        private IEnumerator RotateCurrentSkyBoxRoutine()
        {
            float currentRotation = 0f;
            while (true)
            {
                yield return null;
                currentRotation += 1 * _rotationScale;
                _skyBoxView.RotationAmount.Value = currentRotation;
            }
        }

        private IEnumerator RunDayTimeRoutine()
        {
            _rotationCoroutine = StartCoroutine(RotateCurrentSkyBoxRoutine());
            yield return StartCoroutine(_dayTimeHandler.RunTimeRoutine(_skyBoxView, _timeScale));
            StartCoroutine(RunNightTimeRoutine());
            if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);
        }

        private IEnumerator RunNightTimeRoutine()
        {
            _rotationCoroutine = StartCoroutine(RotateCurrentSkyBoxRoutine());
            yield return
                StartCoroutine(_nightTimeHandler.RunTimeRoutine(_skyBoxView, _timeScale));
            StartCoroutine(RunDayTimeRoutine());
            if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);
        }
    }
}
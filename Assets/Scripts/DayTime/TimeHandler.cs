using System.Collections;
using DayTime.Phases;
using UnityEngine;

namespace DayTime
{
    public class TimeHandler : MonoBehaviour
    {
        [Header("Time Phases")] 
        [SerializeField] private IncreaseLightPhase _increaseLightPhase;
        [SerializeField] private DecreaseLightPhase _decreaseLightPhase;

        private TimePhase _currentPhase;
        
        public IEnumerator RunTimeRoutine(SkyBoxView skyBoxView, float timeScale)
        {
            yield return StartCoroutine(
                _increaseLightPhase.RunPhaseRoutine(skyBoxView, timeScale));
            yield return StartCoroutine(_decreaseLightPhase.RunPhaseRoutine(skyBoxView, timeScale));
        }
    }
}
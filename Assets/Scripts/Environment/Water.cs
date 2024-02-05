using System.Collections;
using CharacterStatsSystem;
using Player_Controller;
using UnityEngine;
using Vehicle;

namespace Environment
{
    public class Water : MonoBehaviour
    {
        private float _waveHeight = 0f;
        private bool _isRestoringOxygen = false;
        private Coroutine _oxygenCoroutine;
        
        private CharacterStats _characterStats;

        private void OnEnable()
        {
            CharacterStatsEventsContainer.OnCharacterStatsAssign += Init;
        }

        private void Init(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isRestoringOxygen = false;
                _oxygenCoroutine = StartCoroutine(RemoveOxygenOverTime());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isRestoringOxygen = true;
                if (_oxygenCoroutine != null)
                {
                    StopCoroutine(_oxygenCoroutine);
                }
                StartCoroutine(RestoreOxygenToFull());
            }

            if (other.CompareTag("Boat"))
            {
                var boat = other.GetComponent<Boat>();
                if (boat != null)
                {
                    boat.Float(_waveHeight, false);
                }
            }
        }

        // Coroutine to gradually restore oxygen over time
        private IEnumerator RemoveOxygenOverTime()
        {
            while (!_isRestoringOxygen)
            {
                yield return new WaitForSeconds(0.25f);
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Oxygen, 1);
            }
        }
        
        private IEnumerator RestoreOxygenToFull()
        {
            while (_characterStats.Oxygen.Value < 100)
            {
                yield return new WaitForSeconds(0.05f);
                CharacterStatsEventsContainer.OnCharacterStatAdded.Invoke(CharacterStatType.Oxygen, 1);
            }
        }
    }
}

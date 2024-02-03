using CharacterStatsSystem;
using Player_Controller;
using UnityEngine;
using Vehicle;

namespace Environment
{
    public class Water : MonoBehaviour
    {
        private float _waveHieght = 0f; //Could be used later for water
        private bool _isRestoringOxygen = false;

        private void Update()
        {
            if (_isRestoringOxygen)
            {
                CharacterStatsEventsContainer.OnCharacterStatAdded.Invoke(CharacterStatType.Oxygen, (int)(1 * Time.fixedDeltaTime));
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
            {
                _isRestoringOxygen = false;
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Oxygen, 10);
                var move = other.GetComponent<PlayerController>();
                move.IsSwimming = true;
            }

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Boat"))
            {
                var boat = other.GetComponent<Boat>();
                if (boat != null) boat.Float(_waveHieght, true);
            }

            if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
            {
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Oxygen, (int)(1 * Time.fixedDeltaTime));
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
            {
                _isRestoringOxygen = true;
                var move = other.GetComponent<PlayerController>();
                move.IsSwimming = false;
            }

            if (other.CompareTag("Boat"))
            {
                var boat = other.GetComponent<Boat>();
                if (boat != null)
                {
                    boat.Float(_waveHieght, false);
                }
            }
        }
    }
}
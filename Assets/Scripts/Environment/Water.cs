using Character_Stats;
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

                CharacterStats.Singleton.PlusStat(CharacterStatType.Oxygen, 1 * Time.fixedDeltaTime);
                if (CharacterStats.Singleton.Oxygen >= CharacterStats.Singleton.CurrentOxygen)
                {
                    CharacterStats.Singleton.SetActiveOxygen(false);
                    _isRestoringOxygen = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
            {
                _isRestoringOxygen = false;
                CharacterStats.Singleton.MinusStat(CharacterStatType.Oxygen, 10);
                CharacterStats.Singleton.SetActiveOxygen(true);
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
                CharacterStats.Singleton.MinusStat(CharacterStatType.Oxygen, 1 * Time.fixedDeltaTime);
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
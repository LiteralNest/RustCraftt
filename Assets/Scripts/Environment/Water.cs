using System;
using Player_Controller;
using UnityEngine;

public class Water : MonoBehaviour
{

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
        CharacterStats.Singleton.MinusStat(CharacterStatType.Oxygen, 1 * Time.fixedDeltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            _isRestoringOxygen = true;
            var move = other.GetComponent<PlayerController>();
            move.IsSwimming = false;
        }
    }
}
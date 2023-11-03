using System;
using Player_Controller;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            CharacterStats.Singleton.MinusStat(CharacterStatType.Oxygen, 10);
            CharacterStats.Singleton.SetActiveOxygen();
            var move = other.GetComponent<PlayerController>();
            Debug.Log("Swim");
            move.IsSwimming = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CharacterStats.Singleton.MinusStat(CharacterStatType.Oxygen, 1 *Time.fixedDeltaTime);
        Debug.Log("Stay");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            var move = other.GetComponent<PlayerController>();
            CharacterStats.Singleton.PlusStat(CharacterStatType.Oxygen, 1);
            Debug.Log("Exit");
            move.IsSwimming = false;
        }
    }
}

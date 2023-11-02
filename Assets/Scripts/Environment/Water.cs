using System;
using Player_Controller;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            var move = other.GetComponent<PlayerController>();
            
            move.IsSwimming = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CharacterStats.Singleton.MinusStat(CharacterStatType.Oxygen, 10);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            var move = other.GetComponent<PlayerController>();
            CharacterStats.Singleton.PlusStat(CharacterStatType.Oxygen, 1);
            move.IsSwimming = false;
        }
    }
}

using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            var move = other.GetComponent<PlayerController>();
            move.IsSwimming = true;
            Debug.Log("Swim");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            var move = other.GetComponent<PlayerController>();
            move.IsSwimming = false;
        }
    }
}

using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private float _climbForce = 10.0f;
    [SerializeField] private float _distanceToTrigger = 1.5f;

    private bool isClimbing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClimbing = true;
            if (isClimbing && other.CompareTag("Player"))
            {
                Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    Vector3 climbForceVector = Vector3.up * _climbForce;
                    playerRigidbody.AddForce(climbForceVector, ForceMode.Impulse);
                }
            }
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (isClimbing && other.CompareTag("Player"))
        {
            isClimbing = false;
        }
    }
}

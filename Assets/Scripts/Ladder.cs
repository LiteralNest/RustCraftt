using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Ladder : MonoBehaviour
{
    private bool isClimbing = false;
    private Rigidbody playerRigidbody;
    private Vector3 moveDirection;

    public float climbSpeed = 3.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClimbing = true;
            playerRigidbody = other.GetComponent<Rigidbody>();
            playerRigidbody.useGravity = false;
            moveDirection = Vector3.zero; // Инициализируем направление движения
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isClimbing && other.CompareTag("Player"))
        {
            isClimbing = false;
            playerRigidbody.useGravity = true;
            moveDirection = Vector3.zero; // Обнуляем направление движения при выходе
        }
    }

    private void Update()
    {
        if (isClimbing)
        {
            float verticalMovement = Input.GetAxis("Vertical");

            moveDirection = transform.up * verticalMovement * climbSpeed;
            playerRigidbody.velocity = moveDirection;
        }
    }
}
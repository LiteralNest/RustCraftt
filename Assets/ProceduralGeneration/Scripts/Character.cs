using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
   [SerializeField] private float _movingYSpeed = 5;
       [SerializeField] private float _movingSpeed = 5;
       [SerializeField] private float _rotationSpeed = 5;
       [SerializeField] private Rigidbody _rb;
    
       private void Update()
       {
          Walk();
          HandleInput();
          RotatePlayer();
       }

       private void RotatePlayer()
       {
          if (Input.GetKey(KeyCode.Q))
          {
             RotatePlayer(-1f); // Rotate left
          }
          else if (Input.GetKey(KeyCode.E))
          {
             RotatePlayer(1f); // Rotate right
          }
       }

       private void RotatePlayer(float direction)
       {
          Vector3 rotation = new Vector3(0f, direction * _rotationSpeed * Time.deltaTime, 0f);
          transform.Rotate(rotation);
       }
       private void HandleInput()
       {
          if (Input.GetKey(KeyCode.Space))
             Fly(Vector3.up);
          else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
             Fly(Vector3.down);
       }
    
       private void Walk()
       {
    
          float horizontalInput = Input.GetAxis("Horizontal");
          float verticalInput = Input.GetAxis("Vertical");
    
          Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
    
          movementDirection.Normalize();
          transform.Translate(movementDirection * _movingSpeed * Time.deltaTime, Space.Self);
       }
       
       private void Fly(Vector3 direction)
       {
          transform.Translate(direction * _movingYSpeed * Time.deltaTime);
       }
    
       private Vector3 GetMovementDirection(Vector3 movementInput)
       {
          return transform.right * movementInput.x + transform.forward * movementInput.z;
       }
    
       
       public void Move(Vector3 movementInput)
       {
          Vector3 movementDirection = GetMovementDirection(movementInput);
          movementDirection.y = 0;
          transform.Translate(movementDirection * _movingSpeed * Time.deltaTime, Space.World);
       }
}
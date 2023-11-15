using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicle
{
    public class Copter : BaseVehicle
    {
        [SerializeField] private float _pushForce = 100f;
        [SerializeField] private float _heightAdjustSpeed = 5f;

        [Header("Angels")] 
        [SerializeField] private float _tiltAngleX;
        [SerializeField] private float _tiltAngelZ;
        
        private float _power;
        private float _tilt;
        private float _currentHeight;
        private bool _isGrounded;

        public void Push(Vector3 pushDirection)
        {
            VehicleRb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
        }

        public void Move(Vector2 moveInput)
        {
            float forwardMovement = Mathf.Clamp(moveInput.y, 0, 1f);
            float sidewaysMovement = moveInput.x;

            var movement = new Vector3(0f, 0f, forwardMovement);
            var rotationX = sidewaysMovement * RotationSpeed * Time.fixedDeltaTime;
            var rotationY = moveInput.y * RotationSpeed * Time.fixedDeltaTime;

            if (_isGrounded)
            {
                transform.Rotate(0f, rotationX, 0f);

                if (forwardMovement > 0)
                {
                    transform.Translate(movement * MoveSpeed * Time.deltaTime, Space.Self);
                }
            }
            else
            {
                // Air movement logic
                float forwardMovementAir = moveInput.y;
                var movementAir = new Vector3(0f, 0f, forwardMovement);
                // Tilt based on input direction
                float tiltX = Mathf.Lerp(_tilt, forwardMovementAir * _tiltAngleX, Time.fixedDeltaTime);
                float tiltZ = Mathf.Lerp(_tilt, sidewaysMovement * _tiltAngelZ, Time.fixedDeltaTime);

                _tilt = Mathf.Lerp(_tilt, 0f, Time.deltaTime * 2f);
                transform.Rotate(0f, rotationX, 0f, Space.Self);
                if (movement.x != 0)
                {
                    transform.Rotate(tiltX, 0f, 0f, Space.Self);
                }
                

                if (forwardMovementAir != 0)
                {
                    // Translate forward based on y input
                    transform.Rotate(0f, 0f, tiltZ);
                    transform.Translate(movementAir * MoveSpeed * Time.deltaTime, Space.Self);
                }
               
            }
        }



        public void TakeOff()
        {
            VehicleRb.isKinematic = true;

            transform.Translate(Vector3.up * 5f);
        }

        public void IncreaseHeight()
        {
            if (_isGrounded) 
                TakeOff();
            else 
                transform.Translate(Vector3.up * _heightAdjustSpeed * Time.deltaTime);
        }

        public void DecreaseHeight()
        {
            transform.Translate(Vector3.down * _heightAdjustSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("collision detected!");
                _isGrounded = true;
                VehicleRb.isKinematic = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = false;
                VehicleRb.isKinematic = true;
                Debug.Log("collision exit!");
            }
        }
    }
}

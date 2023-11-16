using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Vehicle
{
    public class Copter : BaseVehicle
    {
        [SerializeField] private float _pushForce = 100f;
        [SerializeField] private float _heightAdjustSpeed = 5f;
        [SerializeField] private float _stabilizeSpeed = 10f;


        [Header("Angels")] 
        [SerializeField] private float _tiltAngleX;

        [SerializeField] private float _tiltAngelZ;
        [SerializeField] private float _tiltAngleY;
        private float _power;
        private float _tilt;


        // private float _currentHeight;
        private bool _isGrounded;

        public void Push(Vector3 pushDirection)
        {
            VehicleRb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
        }

        public void Stabilize()
        {
            
            // Smoothly return to zero tilt on X and Z axes
            var currentYRotation = transform.rotation.eulerAngles.y;
            
            _tilt = Mathf.Lerp(_tilt, 0f, Time.deltaTime * _stabilizeSpeed);


            Quaternion targetRotation = Quaternion.Euler(-_tilt, currentYRotation, -_tilt);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _stabilizeSpeed);
        }
        public void Move(Vector2 moveInput)
        {
            float forwardMovement = Mathf.Clamp(moveInput.y, 0, 1f);
            float sidewaysMovement = moveInput.x;

            var movement = new Vector3(0f, 0f, forwardMovement);
            var rotationX = sidewaysMovement * RotationSpeed * Time.fixedDeltaTime;
            
            _tilt = Mathf.Lerp(_tilt, 0f, Time.deltaTime * 2f);

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
                
                var movementAir = new Vector3(0f, 0f, forwardMovementAir);

                _tilt = Mathf.Lerp(_tilt, 0f, Time.deltaTime * 2f);
                // Tilt based on input direction
                float tiltX = Mathf.Lerp(_tilt, forwardMovementAir * _tiltAngleX, Time.fixedDeltaTime);
                float tiltZ = Mathf.Lerp(_tilt, sidewaysMovement * _tiltAngelZ, Time.fixedDeltaTime);
                float tiltY = Mathf.Lerp(_tilt, forwardMovementAir * _tiltAngleY, Time.fixedDeltaTime);
                
                transform.Rotate(0f, rotationX, 0f, Space.Self);
                
                if (sidewaysMovement != 0)
                {
                    transform.Rotate(tiltX, 0f, tiltZ, Space.Self);
                    Debug.Log("Rotate");
                }
            
                
                if (forwardMovementAir > 0)
                {
                    // Translate forward based on y input
                    transform.Rotate(0, tiltY, 0f, Space.Self);
                    transform.Translate(movementAir * MoveSpeed * Time.deltaTime, Space.Self);
                    Debug.Log("Forward");
                }
                else if (forwardMovementAir < 0)
                {
                    transform.Rotate(0, -tiltY, 0f, Space.Self);
                    transform.Translate(-movementAir * MoveSpeed * Time.deltaTime, Space.Self);
                    Debug.Log("back");
                }
            }
        }



        public void TakeOff()
        {
            VehicleRb.isKinematic = true;

            transform.Translate(Vector3.up * 3f);
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
            if (!_isGrounded) 
                transform.Translate(Vector3.down * _heightAdjustSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
                VehicleRb.isKinematic = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded = false;
                VehicleRb.isKinematic = true;
               
            }
        }
    }
}

using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Vehicle
{
    public class CopterController : NetworkBehaviour
    {
        [SerializeField] private Copter _copter;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, 0f);
        [SerializeField] private PlayerInput _copterInput;
        
        public bool IsMoving { get; set; }
        private bool _isNearVehicle = false;
        private bool _isSittingInVehicle;
        private bool _hasGainedHeight;

        private PlayerController _playerController;
        private Vector2 _moveInput;
        private float _currentHeight;
        private RigidbodyConstraints _rbConstraints;
        

        private void Awake()
        {
            _copterInput.enabled = false;
            _currentHeight = 0f;
        }

        private void FixedUpdate()
        {
            if (IsMoving)
            {
                _copter.Move(_moveInput);
            }
        }

        private void OnGUI()
        {
            if (!_isSittingInVehicle)
            {
                if (_isNearVehicle && _playerController != null)
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 200, 100), "Push Boat"))
                    {
                        PushVehicle();
                    }
                    if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 75, 200, 100), "Sit in Boat"))
                    {
                        SitInVehicle();
                    }
                }
            }
            else if (_isSittingInVehicle)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 75, 200, 100), "Stand from boat"))
                {
                    StandUpFromVehicle();
                }

                // Add buttons for height adjustment in the center-right position
                float buttonWidth = 150f;
                float buttonHeight = 150f;

                if (GUI.Button(new Rect(Screen.width - buttonWidth - 10, Screen.height / 2 - buttonHeight / 2, buttonWidth, buttonHeight), "Up"))
                {
                    OnHeightUp();
                }

                if (GUI.Button(new Rect(Screen.width - buttonWidth - 10, Screen.height / 2 + buttonHeight / 2 + 20, buttonWidth, buttonHeight), "Down"))
                {
                    OnHeightDown();
                }
            }
        }

        private void OnHeightUp()
        {
            _copter.IncreaseHeight();
        }

        private void OnHeightDown()
        {
            _copter.DecreaseHeight();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            IsMoving = context.performed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isNearVehicle = true;
                _playerController = collision.gameObject.GetComponent<PlayerController>();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isNearVehicle = false;
                _playerController = null;
            }
        }
        
        private void PushVehicle()
        {
            if (_playerController != null)
            {
                Vector3 pushDirection = (_playerController.transform.forward).normalized;
                
                _copter.Push(pushDirection);
            }
        }

        private void SitInVehicle()
        {
            if (!IsServer) return;
            _isSittingInVehicle = true;
            
            _playerController.GetComponent<NetworkObject>().TrySetParent(_copter.transform);
            _playerController.transform.position = _copter.transform.position + _offset;
            
            _rbConstraints = _playerController.GetComponent<Rigidbody>().constraints;
            SetPlayerPhysicInVehicle();
            
            _playerController.enabled = false;
            _copterInput.enabled = true;
        }

        private void StandUpFromVehicle()
        {
            if (!IsServer) return;
            _isSittingInVehicle = false;
            _playerController.GetComponent<NetworkObject>().TryRemoveParent(true);
            
            var rb =_playerController.GetComponent<Rigidbody>();
            rb.constraints = _rbConstraints;
            rb.useGravity = true;
            
            _playerController.enabled = true;
            _copterInput.enabled = false;
        }

        private void SetPlayerPhysicInVehicle()
        {
            var rb =_playerController.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = true;
        }
    }
}

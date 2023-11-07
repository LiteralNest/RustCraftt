using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class BoatController : NetworkBehaviour
    {
        [SerializeField] private Boat _boat;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, 0f);
        [SerializeField] private PlayerInput _boatInput;
        public bool IsMoving { get; set; }
        private bool _isNearBoat = false;
        private bool _isSittingInBoat;
        
        private PlayerController _playerController;
        private Vector2 _moveInput;
        private RigidbodyConstraints _rbConstraints;
        private void Awake()
        {
            _boatInput.enabled = false;
            
        }

        private void FixedUpdate()
        {
            if (IsMoving && _boat.IsFloating)
            {
                _boat.Move(_moveInput);
            }
        }

        private void OnGUI()
        {
            if (!_isSittingInBoat)
            {
                if (_isNearBoat && _playerController != null)
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 200, 100), "Push Boat"))
                    {
                        PushBoat();
                    }
                    if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 75, 200, 100), "Sit in Boat"))
                    {
                        SitInBoat();
                    }
                }
            }
            else if (_isSittingInBoat)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 75, 200, 100), "Stand from boat"))
                {
                    StandUpFromBoat();
                }
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            IsMoving = context.performed;
        }

        #region InteractionWithBoat

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isNearBoat = true;
                _playerController = collision.gameObject.GetComponent<PlayerController>();
                
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isNearBoat = false;
                _playerController = null;
            }
        }

        private void PushBoat()
        {
            if (_playerController != null)
            {
                Vector3 pushDirection = (_playerController.transform.forward).normalized;
                
                _boat.Push(pushDirection);
            }
        }

        #endregion

        private void SitInBoat()
        {
            if (!IsServer) return;
            _isSittingInBoat = true;
            
            _playerController.GetComponent<NetworkObject>().TrySetParent(_boat.transform);
            _playerController.transform.position = _boat.transform.position + _offset;
            _rbConstraints = _playerController.GetComponent<Rigidbody>().constraints;
            
            SetPlayerPhysicInBoat();
            
            _playerController.enabled = false;
            _boatInput.enabled = true;
        }

        private void StandUpFromBoat()
        {
            if (!IsServer) return;
            _isSittingInBoat = false;
            _playerController.GetComponent<NetworkObject>().TryRemoveParent(true);
            var rb =_playerController.GetComponent<Rigidbody>();
            rb.constraints = _rbConstraints;
            rb.useGravity = true;
            
            _playerController.enabled = true;
            _boatInput.enabled = false;
        }

        private void SetPlayerPhysicInBoat()
        {
            var rb =_playerController.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = true;
        }
    }
}
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class HorseController : NetworkBehaviour
    {
        [SerializeField] private Horse _horse;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 3f, 0f);
        [SerializeField] private PlayerInput _horseInput;
        
        public bool IsMoving { get; set; }
        private bool _isNearHorse = false;
        private bool _isSittingOnHorse;
        
        private PlayerController _playerController;
        private Vector2 _moveInput;
        private RigidbodyConstraints _rbConstraints;
        
        private void Awake()
        {
            _horseInput.enabled = false;
        }
        private void FixedUpdate()
        {
            if (IsMoving)
            {
                _horse.Move(_moveInput);
            }
        }
        private void OnGUI()
        {
            if (!_isSittingOnHorse)
            {
                if (_isNearHorse && _playerController != null)
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 75, 200, 100), "Siton horse"))
                    {
                        SitOnHorse();
                    }
                }
            }
            else if (_isSittingOnHorse)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Stand from horse"))
                {
                    StandUpFromHorse();
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 50, 200, 100), "Stand from horse"))
                {
                    _horse.Jump();
                }
            }
        }

        #region InputMap
        public void OnMove(InputAction.CallbackContext context)
        {
            IsMoving = true;
            _moveInput = context.ReadValue<Vector2>();
        }
        #endregion

        #region Trigger

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ground"))
                _horse.Jump();
        }

        private void OnTriggerExit(Collider other)
        {
            //
            // var ground = other.CompareTag("Ground");
            // if (ground)
            //     ground = false;
        }

        #endregion
        #region Colision
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isNearHorse = true;
                _playerController = collision.gameObject.GetComponent<PlayerController>();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isNearHorse = false;
                _playerController = null;
            }
        }
        #endregion
        #region InteractionWithVehiclce
        private void SitOnHorse()
        {
            if (!IsServer) return;
            _isSittingOnHorse = true;
            
            _playerController.GetComponent<NetworkObject>().TrySetParent(_horse.transform);
            _playerController.transform.position = _horse.transform.position + _offset;
            _rbConstraints = _playerController.GetComponent<Rigidbody>().constraints;
            
            SetPlayerPhysicOnHorse();
            
            _playerController.enabled = false;
            _horseInput.enabled = true;
        }
        private void StandUpFromHorse()
        {
            if (!IsServer) return;
            _isSittingOnHorse = false;
            _playerController.GetComponent<NetworkObject>().TryRemoveParent(true);
            var rb =_playerController.GetComponent<Rigidbody>();
            rb.constraints = _rbConstraints;
            rb.useGravity = true;
            
            _playerController.enabled = true;
            _horseInput.enabled = false;
        }

        private void SetPlayerPhysicOnHorse()
        {
            var rb =_playerController.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = true;
        }
        #endregion
     
    }
}
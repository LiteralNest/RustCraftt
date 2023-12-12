using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class CopterController : NetworkBehaviour, IVehicleController
    {
        [SerializeField] private Copter _copter;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, 0f);
        [SerializeField] private PlayerInput _copterInput;

        public bool IsMoving { get; set; }
        private bool _isSittingInVehicle;
        private bool _hasGainedHeight;
        
        private Vector2 _moveInput;
        private RigidbodyConstraints _rbConstraints;
        private Quaternion _cachedCameraPosition;

        private void Awake()
            => _copterInput.enabled = false;

        private void Update()
        {
            if (_moveInput.x != 0) return;
            _copter.Stabilize();
        }

        private void FixedUpdate()
        {
            if (!IsMoving) return;
            _copter.Move(_moveInput);
        }

        #region IVehicleController

        public bool CanBePushed() => false;

        public void Push(PlayerNetCode playerNetCode)
        {
        }

        public bool CanStand() => _isSittingInVehicle;

        public void StandUp(PlayerNetCode playerNetCode)
        {
            _isSittingInVehicle = false;
            var player = playerNetCode;
            player.GetComponent<NetworkObject>().TryRemoveParent(true);

            var rb = player.GetComponent<Rigidbody>();
            rb.constraints = _rbConstraints;
            rb.useGravity = true;

            player.transform.rotation = _cachedCameraPosition;

            player.GetComponent<PlayerController>().enabled = true;
            _copterInput.enabled = false;
        }

        public bool CanSit() => !_isSittingInVehicle;

        private void SetPlayerPhysicInVehicle(PlayerNetCode playerNetCode)
        {
            var player = playerNetCode;
            var rb = player.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = true;
        }

        public void SitIn(PlayerNetCode playerNetCode)
        {
            _isSittingInVehicle = true;

            var player = playerNetCode;
            
            _cachedCameraPosition = player.transform.rotation;
            player.GetComponent<NetworkObject>().TrySetParent(_copter.transform);
            player.transform.position = _copter.transform.position + _offset;

            _rbConstraints = player.GetComponent<Rigidbody>().constraints;
            SetPlayerPhysicInVehicle(playerNetCode);

            player.GetComponent<PlayerController>().enabled = false;
            _copterInput.enabled = true;
        }

        public bool CanMoveUp() => _isSittingInVehicle;

        public void MoveUp(PlayerNetCode playerNetCode)
            => _copter.IncreaseHeight();

        public bool CanMoveDown() => _isSittingInVehicle;
        
        public void MoveDown(PlayerNetCode playerNetCode)
            => _copter.DecreaseHeight();

        #endregion

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            IsMoving = context.performed;
        }

        // private void PushVehicle()
        // {
        //     if (_playerController != null)
        //     {
        //         Vector3 pushDirection = (_playerController.transform.forward).normalized;
        //
        //         _copter.Push(pushDirection);
        //     }
        // }
    }
}
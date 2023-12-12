using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class BoatController : NetworkBehaviour, IVehicleController
    {
        [SerializeField] private Boat _boat;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, 0f);
        [SerializeField] private PlayerInput _boatInput;
        public bool IsMoving { get; set; }
        private bool _isSittingInBoat;

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

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            IsMoving = context.performed;
        }


        private void SetPlayerPhysicInBoat(PlayerNetCode playerNetCode)
        {
            var rb = playerNetCode.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = true;
        }

        public bool CanBePushed()
            => true;

        public void Push(PlayerNetCode playerNetCode)
        {
            var player = playerNetCode;
            if (player != null)
            {
                Vector3 pushDirection = (player.transform.forward).normalized;

                _boat.Push(pushDirection);
            }
        }

        public bool CanStand()
            => _isSittingInBoat;

        public void StandUp(PlayerNetCode playerNetCode)
        {
            if (!IsServer) return;
            _isSittingInBoat = false;
            var player = playerNetCode;
            player.GetComponent<NetworkObject>().TryRemoveParent(true);
            var rb = player.GetComponent<Rigidbody>();
            rb.constraints = _rbConstraints;
            rb.useGravity = true;

            _boatInput.enabled = false;
        }

        public bool CanSit()
            => !_isSittingInBoat;

        public void SitIn(PlayerNetCode playerNetCode)
        {
            if (!IsServer) return;
            _isSittingInBoat = true;
            var player = playerNetCode;
            player.GetComponent<NetworkObject>().TrySetParent(_boat.transform);
            player.transform.position = _boat.transform.position + _offset;
            _rbConstraints = player.GetComponent<Rigidbody>().constraints;

            SetPlayerPhysicInBoat(playerNetCode);
            player.GetComponent<PlayerController>().enabled = false;
            _boatInput.enabled = true;
        }

        public bool CanMoveUp()
            => false;

        public void MoveUp(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
        }

        public bool CanMoveDown()
            => false;

        public void MoveDown(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
        }
    }
}
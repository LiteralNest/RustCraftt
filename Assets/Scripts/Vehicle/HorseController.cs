using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class HorseController : NetworkBehaviour, IVehicleController
    {
        [SerializeField] private Horse _horse;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 3f, 0f);
        [SerializeField] private PlayerInput _horseInput;
        
        public bool IsMoving { get; set; }
        private bool _isNearHorse = false;
        private bool _isSittingOnHorse;
        
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

        #region InputMap
        public void OnMove(InputAction.CallbackContext context)
        {
            IsMoving = true;
            _moveInput = context.ReadValue<Vector2>();
        }
        #endregion


        private void SetPlayerPhysicOnHorse()
        {
            var rb =  PlayerNetCode.Singleton.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = true;
        }

        #region IVehicleController

        public bool CanBePushed()
            => false;

        public void Push()
        {}

        public bool CanStand()
            => _isSittingOnHorse;

        public void StandUp()
        {
            if (!IsServer) return;
            _isSittingOnHorse = false;
            var player = PlayerNetCode.Singleton;
            player.GetComponent<NetworkObject>().TryRemoveParent(true);
            var rb =player.GetComponent<Rigidbody>();
            rb.constraints = _rbConstraints;
            rb.useGravity = true;
            
            player.GetComponent<PlayerController>().enabled = true;
            _horseInput.enabled = false;
        }

        public bool CanSit()
            => !_isSittingOnHorse;

        public void SitIn()
        {
            if (!IsServer) return;
            _isSittingOnHorse = true;

            var player = PlayerNetCode.Singleton;
            player.GetComponent<NetworkObject>().TrySetParent(_horse.transform);
            player.transform.position = _horse.transform.position + _offset;
            _rbConstraints = player.GetComponent<Rigidbody>().constraints;
            
            SetPlayerPhysicOnHorse();
            
            player.GetComponent<PlayerController>().enabled = false;
            _horseInput.enabled = true;
        }

        public bool CanMoveUp()
            => false;
        public void MoveUp()
        {
            throw new System.NotImplementedException();
        }

        public bool CanMoveDown()
            => false;

        public void MoveDown()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
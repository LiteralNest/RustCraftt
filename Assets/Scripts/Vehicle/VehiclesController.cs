using Player_Controller;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Vehicle
{
    public class VehiclesController : NetworkBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private PlayerNetCode _playerNetCode;
        [SerializeField] private CharacterUIHandler _uiHandler;
        [Header("UI")] [SerializeField] private GameObject _sitInButton;
        [SerializeField] private GameObject _standUpButton;
        [SerializeField] private GameObject _pushButton;
        [SerializeField] private GameObject _moveUpButton;
        [SerializeField] private GameObject _moveDownButton;

        private IVehicleController _sittingInVehicle;
        private IVehicleController _vehicleController;

        private void Start()
            => ResetButtons();

        public void SetVehicleController(IVehicleController value)
        {
            _vehicleController = value;
            if (_vehicleController == null)
                ResetButtons();
            else
                TrySetButtons();
        }

        private void ResetButtons()
        {
            _sitInButton.SetActive(false);
            _pushButton.SetActive(false);
            _standUpButton.SetActive(false);
        }

        private void TrySetButtonValue(bool value, GameObject target)
        {
            if (target.activeSelf != value)
                target.SetActive(value);
        }

        private void TrySetButtons()
        {
            TrySetButtonValue(_vehicleController.CanBePushed(), _pushButton);
        }

        public void Push()
            => _vehicleController.Push(_playerNetCode);
        
        public void HandleMoveUp(bool value)
            => _sittingInVehicle.HandleMovingUp(value);

        public void HandleMoveDown(bool value)
            => _sittingInVehicle.HandleMovingDown(value);
    }
}
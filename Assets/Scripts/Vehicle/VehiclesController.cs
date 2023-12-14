using Player_Controller;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Vehicle
{
    public class VehiclesController : NetworkBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private PlayerNetCode _playerNetCode;

        [SerializeField] private CharacterUIHandler _uiHandler;
        [Header("UI")] [SerializeField] private GameObject _pushButton;
        [SerializeField] private GameObject _moveUpButton;
        [SerializeField] private GameObject _moveDownButton;

        [Header("Engine UI")] [SerializeField] private GameObject _enginePanel;
        [SerializeField] private GameObject _activeEngineButton;
        [SerializeField] private GameObject _deactiveEngineButton;
        [SerializeField] private GameObject _cannotActiveEngineButton;

        private VehicleController _sittingInVehicle;
        private VehicleController _vehicleController;

        public void SetVehicleController(VehicleController value)
        {
            _vehicleController = value;
            if (_vehicleController == null)
                TrySetButtonValue(false, _pushButton);
            else
                TrySetButtonValue(_vehicleController.CanBePushed(), _pushButton);
        }

        private void TrySetButtonValue(bool value, GameObject target)
        {
            if (target.activeSelf != value)
                target.SetActive(value);
        }

        private void SetEngineUI(VehicleController vehicleController)
        {
            _deactiveEngineButton.SetActive(false);
            _cannotActiveEngineButton.SetActive(false);
            _activeEngineButton.SetActive(false);
            if (vehicleController.EngineTurnedOn)
                _deactiveEngineButton.SetActive(true);
            else if (!vehicleController.EnoughFuel())
                _cannotActiveEngineButton.SetActive(true);
            else
                _activeEngineButton.SetActive(true);
        }

        public void SitIn(VehicleController vehicleController)
        {
            _sittingInVehicle = vehicleController;
            TrySetButtonValue(_sittingInVehicle.CanMoveUp(), _moveUpButton);
            TrySetButtonValue(_sittingInVehicle.CanMoveDown(), _moveDownButton);
            var hasEngine = _sittingInVehicle.HasEngine();
            TrySetButtonValue(hasEngine, _enginePanel);
            if (hasEngine)
                SetEngineUI(vehicleController);
        }

        public void StandUp()
        {
            _sittingInVehicle = null;
            TrySetButtonValue(false, _moveUpButton);
            TrySetButtonValue(false, _moveDownButton);
            TrySetButtonValue(false, _enginePanel);
        }

        public void Push()
            => _vehicleController.Push(_playerNetCode);

        public void HandleMoveUp(bool value)
            => _sittingInVehicle.HandleMovingUp(value);

        public void HandleMoveDown(bool value)
            => _sittingInVehicle.HandleMovingDown(value);

        public void TurnEngine(bool value)
        {
            if(!value)
                _sittingInVehicle.TurnOff();
            _sittingInVehicle.EngineTurnedOn = value;
            SetEngineUI(_sittingInVehicle);
        }
    }
}
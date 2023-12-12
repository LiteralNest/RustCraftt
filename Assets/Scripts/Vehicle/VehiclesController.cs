using Player_Controller;
using UnityEngine;

namespace Vehicle
{
    public class VehiclesController : MonoBehaviour
    {
        [Header("Attached Components")]
        [SerializeField] private PlayerNetCode _playerNetCode;
        [Header("UI")] 
        [SerializeField] private GameObject _sitInButton;
        [SerializeField] private GameObject _standUpButton;
        [SerializeField] private GameObject _pushButton;
        [SerializeField] private GameObject _moveUpButton;
        [SerializeField] private GameObject _moveDownButton;

        private IVehicleController _sittingInVehicle;
        private IVehicleController _vehicleController;
        
        private void Start()
            => ResetButtons();

        private void Update()
        {
            if(_sittingInVehicle != null)
                TrySetButtonValue(_sittingInVehicle.CanStand(), _standUpButton);
            else
                TrySetButtonValue(false, _standUpButton);
        }
        
        public void SetVehicleController(IVehicleController value)
        { 
            _vehicleController = value;
            if (_vehicleController == null)
                ResetButtons();
            else
                TrySetButtons();
            if(CharacterUIHandler.singleton != null)
                CharacterUIHandler.singleton.ActivateSitAndStandPanel(_vehicleController == null);
        }
        
        private void ResetButtons()
        {
            _sitInButton.SetActive(false);
            _pushButton.SetActive(false);
            _moveUpButton.SetActive(false);
            _moveDownButton.SetActive(false);
        }

        private void TrySetButtonValue(bool value, GameObject target)
        {
            if(target.activeSelf != value)
                target.SetActive(value);
        }
        
        private void TrySetButtons()
        {
            TrySetButtonValue(_vehicleController.CanBePushed(), _pushButton);
            TrySetButtonValue(_vehicleController.CanStand(), _standUpButton);
            TrySetButtonValue(_vehicleController.CanSit(), _sitInButton);
            TrySetButtonValue(_vehicleController.CanMoveUp(), _moveUpButton);
            TrySetButtonValue(_vehicleController.CanMoveDown(), _moveDownButton);
        }

        public void Push()
            => _vehicleController.Push(_playerNetCode);
        
        public void StandUp()
            => _sittingInVehicle.StandUp(_playerNetCode);

        public void SitIn()
        {
            _sittingInVehicle = _vehicleController;
            _vehicleController.SitIn(_playerNetCode);
        }

        public void MoveUp()
            => _vehicleController.MoveUp(_playerNetCode);
        
        public void MoveDown()
            => _vehicleController.MoveDown(_playerNetCode);
    }
}
using UnityEngine;

namespace Vehicle
{
    public class VehiclesController : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private GameObject _sitInButton;
        [SerializeField] private GameObject _standUpButton;
        [SerializeField] private GameObject _pushButton;
        [SerializeField] private GameObject _moveUpButton;
        [SerializeField] private GameObject _moveDownButton;

        private IVehicleController _vehicleController;
        public IVehicleController SetVehiclesController
        {
            set => _vehicleController = value;
        }
        
        private void Start()
        {
            _sitInButton.SetActive(false);
            _standUpButton.SetActive(false);
            _pushButton.SetActive(false);
            _moveUpButton.SetActive(false);
            _moveDownButton.SetActive(false);
        }

        private void TrySetButtonValue(bool value, GameObject target)
        {
            if(target.activeSelf != value)
                target.SetActive(value);
        }
        
        private void Update()
        {
            if (_vehicleController == null)
            {
                MainUiHandler.singleton.ActivateSitAndStandPanel(true);
                return;
            }
            MainUiHandler.singleton.ActivateSitAndStandPanel(false);
            TrySetButtonValue(_vehicleController.CanBePushed(), _pushButton);
            TrySetButtonValue(_vehicleController.CanStand(), _standUpButton);
            TrySetButtonValue(_vehicleController.CanSit(), _sitInButton);
            TrySetButtonValue(_vehicleController.CanMoveUp(), _moveUpButton);
            TrySetButtonValue(_vehicleController.CanMoveDown(), _moveDownButton);
        }

        public void Push()
            => _vehicleController.Push();
        
        public void StandUp()
            => _vehicleController.StandUp();
        
        public void SitIn()
            => _vehicleController.SitIn();
        
        public void MoveUp()
            => _vehicleController.MoveUp();
        
        public void MoveDown()
            => _vehicleController.MoveDown();
    }
}
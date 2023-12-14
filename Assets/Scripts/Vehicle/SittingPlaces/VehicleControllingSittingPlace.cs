using Player_Controller;
using UI;
using UnityEngine;

namespace Vehicle.SittingPlaces
{
    public class VehicleControllingSittingPlace : SittingPlace
    {
        [SerializeField] private VehicleController _vehicle;

        public override void SitIn(PlayerNetCode player)
        {
            base.SitIn(player);
            _vehicle.ActivateInput(true);
            PlayerNetCode.Singleton.VehiclesController.SitIn(_vehicle);
            CharacterUIHandler.singleton.HandleIgnoringVehiclePanels(false);
            CharacterUIHandler.singleton.HandleJoystick(true);
        }
        

        public override void StandUp(PlayerNetCode player)
        {
            base.StandUp(player);
            _vehicle.ActivateInput(false);
            PlayerNetCode.Singleton.VehiclesController.StandUp();
            PlayerNetCode.Singleton.VehiclesController.SetVehicleController(null);
            CharacterUIHandler.singleton.HandleIgnoringVehiclePanels(true);
      
        }

        protected override void TriggerExited(Collider other)
        {
            base.TriggerExited(other);
            _vehicle.TurnOff();
        }
        
        protected override void ResetInput()
            => _vehicle.ActivateInput(false);
    }
}
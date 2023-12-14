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
            CharacterUIHandler.singleton.HandleIgnoringVehiclePanels(false);
        }

        public override void StandUp(PlayerNetCode player)
        {
            base.StandUp(player);
            _vehicle.ActivateInput(false);
            CharacterUIHandler.singleton.HandleIgnoringVehiclePanels(true);
        }

        protected override void ResetInput()
            => _vehicle.ActivateInput(false);
    }
}
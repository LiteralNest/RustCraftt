using Player_Controller;
using UI;

namespace Vehicle.SittingPlaces
{
    public class BaseSittingPlace : SittingPlace
    {
        public override void SitIn(PlayerNetCode player)
        {
            base.SitIn(player);
            CharacterUIHandler.singleton.HandleMovingHudPanels(false);
        }

        public override void StandUp(PlayerNetCode player)
        {
            base.StandUp(player);
            CharacterUIHandler.singleton.HandleMovingHudPanels(true);
        }
    }
}

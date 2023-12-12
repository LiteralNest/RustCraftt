using Player_Controller;

namespace Vehicle
{
    public interface IVehicleController
    {
        public bool CanBePushed();
        public void Push(PlayerNetCode playerNetCode);

        public bool CanStand();
        public void StandUp(PlayerNetCode playerNetCode);

        public bool CanSit();
        public void SitIn(PlayerNetCode playerNetCode);

        public bool CanMoveUp();
        public void MoveUp(PlayerNetCode playerNetCode);
        
        public bool CanMoveDown();
        public void MoveDown(PlayerNetCode playerNetCode);
    }
}
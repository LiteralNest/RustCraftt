namespace Vehicle
{
    public interface IVehicleController
    {
        public bool CanBePushed();
        public void Push();

        public bool CanStand();
        public void StandUp();

        public bool CanSit();
        public void SitIn();

        public bool CanMoveUp();
        public void MoveUp();
        
        public bool CanMoveDown();
        public void MoveDown();
    }
}
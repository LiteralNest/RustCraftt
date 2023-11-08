namespace AI.Animals
{
    public class AC_Peacful : AnimalController
    {
        public override void RefreshList()
        {
            base.RefreshList();
            foreach (var perception in _aIPerceptions)
            {
                ObjectsToInteract.AddRange(perception.GetObjects<PlayerNetCode>());
                ObjectsToInteract.AddRange(perception.GetObjects<AC_Predator>());
            }
        }
    }
}

using DamageSystem;

namespace AI.Animals
{
    public class AC_Predator : AnimalController
    {
        public override void RefreshList()
        {
            base.RefreshList();
            foreach (var perception in _aIPerceptions)
            {
                ObjectsToInteract.AddRange(perception.GetObjects<DamagableBodyPart>());
                ObjectsToInteract.AddRange(perception.GetObjects<AC_Peacful>());
            }
        }
    }
}

using Player_Controller;

namespace ArmorSystem.Backend
{
    public class OneSlotArmorCell : ArmorCell
    {
        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor( netCode);
            _bodyPartsDisplayer.DressArmor(_bodyPartType, _targetMaterial);
            _bodyPartsDisplayer.ReturnArmorsToDefault(_bodyPartType);
        }
    }
}
using Player_Controller;

namespace ArmorSystem.Backend
{
    public class FullDressArmorCell : ArmorCell
    {
        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _targetMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _targetMaterial);
        }
    }  
}
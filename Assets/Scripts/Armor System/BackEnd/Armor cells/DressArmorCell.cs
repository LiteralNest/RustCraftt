using ArmorSystem.Backend;
using Player_Controller;

namespace Armor_System.BackEnd.Armor_cells
{
    public class DressArmorCell : ArmorCell
    {
        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _armorsContainer.DisplayDefaultArmor(netCode);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _targetMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Body, _targetMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _targetMaterial);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Head, _targetMaterial);
        }
    }
}

using Armor_System.BackEnd.Body_Part;
using Player_Controller;
using UnityEngine;

namespace Armor_System.BackEnd.Armor_cells
{
    public class DressArmorCell : ArmorCell
    {
        [SerializeField] private Material _defaultMaterial;
        
        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _armorsContainer.DisplayDefaultArmor();
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _targetMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Body, _targetMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _targetMaterial);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Head, _targetMaterial);
        }

        public override void PutOff(PlayerNetCode netCode)
        {
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _defaultMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Body, _defaultMaterial); 
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _defaultMaterial);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Head, _defaultMaterial);
            //_armorsContainer.DisplayDefaultMaterials();
        }
    }
}

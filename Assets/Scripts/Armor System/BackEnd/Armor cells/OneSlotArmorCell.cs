using Player_Controller;

namespace Armor_System.BackEnd.Armor_cells
{
    public class OneSlotArmorCell : ArmorCell
    {
        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _armorsContainer.DisplayDefaultArmor();
            _bodyPartsDisplayer.DressArmor(_bodyPartType, _targetMaterial);
        }
        
        public override void PutOff(PlayerNetCode netCode)
        {
            foreach(var targetObject in _targetObjects)
                targetObject.SetActive(false);
            foreach(var slot in _inventoryObjects)
                slot.SetActive(false);
        }
    }
}
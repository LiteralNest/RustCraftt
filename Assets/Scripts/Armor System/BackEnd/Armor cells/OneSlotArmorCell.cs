using Player_Controller;

namespace ArmorSystem.Backend
{
    public class OneSlotArmorCell : ArmorCell
    {
        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _armorsContainer.DisplayDefaultArmor();
            _bodyPartsDisplayer.DressArmor(_bodyPartType, _targetMaterial);
        }
        
        public override void PutOff()
        {
            foreach(var targetObject in _targetObjects)
                targetObject.SetActive(false);
            foreach(var slot in _inventoryObjects)
                slot.SetActive(false);
        }
    }
}
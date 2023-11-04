namespace ArmorSystem.Backend
{
    public class OneSlotArmorCell : ArmorCell
    {
        public override void PutOnArmor()
        {
            base.PutOnArmor();
            if (_targetObject != null)
                _targetObject.SetActive(true);
            _bodyPartsDisplayer.DressArmor(_bodyPartType, _targetMaterial);
            _bodyPartsDisplayer.ReturnArmorsToDefault(_bodyPartType);
        }
    }
}
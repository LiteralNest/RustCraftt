namespace ArmorSystem.Backend
{
    public class FullDressArmorCell : ArmorCell
    {
        public override void PutOnArmor()
        {
            base.PutOnArmor();
            _bodyPartsDisplayer.DressArmor(BodyPartType.Head, _targetMaterial);
            _targetObject.SetActive(true);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _targetMaterial);
            _bodyPartsDisplayer.DressArmor( BodyPartType.Legs, _targetMaterial);
        }
    }  
}
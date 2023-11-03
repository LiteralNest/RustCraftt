namespace ArmorSystem
{
    public class FullDressArmorCell : ArmorCell
    {
        public override void PutOnArmor()
        {
            BodyPartsDisplayer.singleton.DressArmor(BodyPartType.Head, _targetMaterial);
            _targetObject.SetActive(true);
            BodyPartsDisplayer.singleton.DressArmor(BodyPartType.Hands, _targetMaterial);
            BodyPartsDisplayer.singleton.DressArmor( BodyPartType.Legs, _targetMaterial);
        }
    }  
}
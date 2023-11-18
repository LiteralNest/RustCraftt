using UnityEngine;

namespace ArmorSystem.Backend
{
    public abstract class ArmorCell: MonoBehaviour
    {
        [Header("Attached scripts")]
        [SerializeField] protected DressedArmorsHandler _dressedArmorsHandler;
        [SerializeField] protected BodyPartsDisplayer _bodyPartsDisplayer;
        [SerializeField] protected BodyPartType _bodyPartType;
        
        [Header("Start Init")]
        [SerializeField] protected Armor _armor;
        public Armor Armor => _armor;
        [SerializeField] protected Material _targetMaterial;
        [SerializeField] protected GameObject _targetObject;

        public virtual void PutOnArmor()
            => _dressedArmorsHandler.DressArmor(_bodyPartType, _armor);

        public virtual void PutOff()
        {
            if(_targetObject)
                _targetObject.SetActive(false);
        }
    }
}
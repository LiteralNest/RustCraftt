using UnityEngine;

namespace ArmorSystem
{
    public abstract class ArmorCell: MonoBehaviour
    {
        //[SerializeField] protected BodyPartType _bodyPartType;
        public Armor Armor => _armor;
        [SerializeField] protected Armor _armor;
 
        [SerializeField] protected Material _targetMaterial;
        [SerializeField] protected GameObject _targetObject;

        public abstract void PutOnArmor();

        public virtual void PutOff()
        {
            if(_targetObject)
                _targetObject.SetActive(false);
        }
    }
}
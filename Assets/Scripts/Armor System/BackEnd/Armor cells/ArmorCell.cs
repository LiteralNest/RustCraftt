using System.Collections.Generic;
using Items_System.Items;
using Player_Controller;
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
        [SerializeField] protected List<GameObject> _targetObjects;
        [SerializeField] protected List<GameObject> _inventoryObjects;

        public virtual void PutOnArmor(PlayerNetCode netCode)
        {
            if (!netCode.IsOwner)
            {
                foreach(var slot in _targetObjects)
                    slot.SetActive(true);
            }
            foreach(var slot in _inventoryObjects)
                slot.SetActive(true);
            _dressedArmorsHandler.DressArmor(_bodyPartType, _armor);
        }

        public void PutOff()
        {
            foreach(var targetObject in _targetObjects)
                targetObject.SetActive(false);
            foreach(var slot in _inventoryObjects)
                slot.SetActive(false);
        }
    }
}
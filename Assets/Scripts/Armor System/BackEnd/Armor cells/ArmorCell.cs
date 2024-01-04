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
        [SerializeField] protected ArmorsContainer _armorsContainer;
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
            DisplayObjects(netCode);
            _dressedArmorsHandler.DressArmor(_bodyPartType, _armor);
        }

        public void DisplayObjects(PlayerNetCode netCode)
        {
            if (!netCode.IsOwner)
            {
                foreach(var slot in _targetObjects)
                    slot.SetActive(true);
            }
            foreach(var slot in _inventoryObjects)
                slot.SetActive(true);
        }

        public virtual void PutOff()
        {
            // foreach(var targetObject in _targetObjects)
            //     targetObject.SetActive(false);
            // foreach(var slot in _inventoryObjects)
            //     slot.SetActive(false);
        }
    }
}
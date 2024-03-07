using System.Collections.Generic;
using Armor_System.BackEnd.Body_Part;
using Player_Controller;
using UnityEngine;

namespace Armor_System.BackEnd.Armor_cells
{
    public class FullDressArmorCell : ArmorCell
    {
        [SerializeField] private List<GameObject> _defaultObjects;
        [SerializeField] private List<GameObject> _inventoryDefaultObjects;
        [SerializeField] private Material _defaultMaterial;

        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _targetMaterial);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _targetMaterial);
            
            if (!netCode.IsOwner)
            {
                foreach (var slot in _defaultObjects)
                    slot.SetActive(false);
            }
            else
            {
                foreach(var slot in _inventoryDefaultObjects)
                    slot.SetActive(false);
            }
        }

        public override void PutOff(PlayerNetCode netCode)
        {
            base.PutOff(netCode);
            foreach (var slot in _targetObjects)
                slot.SetActive(false);
            foreach (var slot in _inventoryObjects)
                slot.SetActive(false);

            if (!netCode.IsOwner)
            {
                foreach (var slot in _defaultObjects)
                    slot.SetActive(true);
            }
            else
            {
                foreach(var slot in _inventoryDefaultObjects)
                    slot.SetActive(true);
            }
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _defaultMaterial);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _defaultMaterial);
            
            _armorsContainer.DisplayDefaultMaterials();
        }
    }
}
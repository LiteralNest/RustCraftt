using System.Collections.Generic;
using Armor_System.BackEnd.Body_Part;
using Player_Controller;
using UnityEngine;

namespace Armor_System.BackEnd.Armor_cells
{
    public class FullDressArmorCell : ArmorCell
    {
        [SerializeField] private List<GameObject> _defaultObjects;

        public override void PutOnArmor(PlayerNetCode netCode)
        {
            base.PutOnArmor(netCode);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Hands, _targetMaterial);
            _bodyPartsDisplayer.DressArmor(BodyPartType.Legs, _targetMaterial);
            foreach (var slot in _targetObjects)
                slot.SetActive(true);
            foreach (var slot in _inventoryObjects)
                slot.SetActive(true);
            foreach (var slot in _defaultObjects)
                slot.SetActive(false);
        }

        public override void PutOff()
        {
            base.PutOff();
            _armorsContainer.DisplayDefaultMaterials();
            foreach (var slot in _targetObjects)
                slot.SetActive(false);
            foreach (var slot in _inventoryObjects)
                slot.SetActive(false);
            foreach (var slot in _defaultObjects)
                slot.SetActive(true);
        }
    }
}
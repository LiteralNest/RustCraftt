using System.Collections.Generic;
using Armor_System.BackEnd.Armor_cells;
using Armor_System.UI;
using Player_Controller;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace OnPlayerItems
{
    public class ArmorsContainer : NetworkBehaviour
    {
        [SerializeField] private PlayerNetCode _playerNetCode;
        [SerializeField] private ArmorCell _defaulArmorCell;
        [SerializeField] private List<ArmorSlotDisplayer> _armorSlotDisplayers = new List<ArmorSlotDisplayer>();
        [SerializeField] private List<ArmorCell> _armorCells = new List<ArmorCell>();
        [SerializeField] private ResistsDisplayer _resistsDisplayer;
        
        public void AssignItem(int itemId)
        {
            _playerNetCode.ActiveArmorId.Value = -1;
            _playerNetCode.ActiveArmorId.Value = itemId;
        }

        public void DisplayDefaultMaterials()
        {
            _defaulArmorCell.PutOnArmor(_playerNetCode);
        }
        
        public void DisplayDefaultArmor()
        {
            _defaulArmorCell.DisplayObjects(_playerNetCode);
        }

        public void ResetItems()
        {
            foreach (var cell in _armorSlotDisplayers)
                cell.ResetInventoryArmor();
        }

        public void CheckFullDressArmor()
        {
            foreach (var cell in _armorSlotDisplayers)
                cell.CheckForFullDress();
        }
        
        public void PutOffItem(int armorId, PlayerNetCode netCode)
        {
            foreach (var armor in _armorCells)
            {
                if (armor.Armor.Id == armorId)
                {
                    armor.PutOff(netCode);
                    if(_resistsDisplayer != null)
                        _resistsDisplayer.DisplayValues();
                    return;
                }
            }
        }

        public void DisplayArmor(int targetArmorId, PlayerNetCode netCode)
        {
            if(targetArmorId == - 1) return;
            foreach (var armor in _armorCells)
            {
                if (armor.Armor.Id != targetArmorId) continue;
                armor.PutOnArmor(netCode);
            }
            if(_resistsDisplayer != null)
                _resistsDisplayer.DisplayValues();
        }
    }
}
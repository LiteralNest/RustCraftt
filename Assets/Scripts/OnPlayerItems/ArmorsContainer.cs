using System.Collections.Generic;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace ArmorSystem.Backend
{
    public class ArmorsContainer : NetworkBehaviour
    {
        [SerializeField] private PlayerNetCode _playerNetCode;
        [SerializeField] private ArmorCell _defaulArmorCell;
        [SerializeField] private List<ArmorCell> _armorCells = new List<ArmorCell>();
        
        public void AssignItem(int itemId)
        {
            _playerNetCode.ActiveArmorId.Value = itemId;
        }

        public void DisplayDefaultArmor(PlayerNetCode netCode)
        {
            _defaulArmorCell.DisplayObjects(netCode);
        }
        
        public void DisplayArmor(int targetArmorId, PlayerNetCode netCode)
        {
            foreach (var armor in _armorCells)
            {
                if (armor.Armor.Id != targetArmorId)
                {
                    armor.PutOff();
                    continue;
                }

                armor.PutOnArmor(netCode);
            }
        }
    }
}
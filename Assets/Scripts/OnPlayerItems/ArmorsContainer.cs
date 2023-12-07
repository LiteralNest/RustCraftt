using System.Collections.Generic;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace ArmorSystem.Backend
{
    public class ArmorsContainer : NetworkBehaviour
    {
        [SerializeField] private PlayerNetCode _playerNetCode;

        [SerializeField] private List<ArmorCell> _armorCells = new List<ArmorCell>();

        private int _cachedArmorId;

        public void AssignItem(int itemId)
        {
            _playerNetCode.ActiveArmorId.Value = itemId;
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

            if (_cachedArmorId != 101 && _cachedArmorId != 0)
                InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(_cachedArmorId, 1, 0);

            _cachedArmorId = targetArmorId;
        }
    }
}
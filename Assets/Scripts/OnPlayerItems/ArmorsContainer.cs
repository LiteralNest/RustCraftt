using System.Collections.Generic;
using UnityEngine;

namespace ArmorSystem.Backend
{
    public class ArmorsContainer : MonoBehaviour
    {
        [SerializeField] private List<ArmorCell> _armorCells = new List<ArmorCell>();
        public void DisplayArmor(Armor targetArmor)
        {
            foreach (var armor in _armorCells)
            {
                if (armor.Armor.Id != targetArmor.Id)
                {
                    armor.PutOff();
                    continue;
                }
                armor.PutOnArmor();
            }
        }
    }
}
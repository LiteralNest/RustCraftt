using System.Collections.Generic;
using UnityEngine;

namespace PlayerDeathSystem.ArmorsSystem
{
    public class CorpesArmorsContainer : MonoBehaviour
    {
        [SerializeField] private List<CorpArmor> _armors = new List<CorpArmor>();

        public void ClearArmors()
        {
            foreach (var armor in _armors)
                armor.Handle(false);
        }

        public void AssignItem(int itemId)
        {
            foreach (var armor in _armors)
            {
                if (armor.ArmorId != itemId) continue;
                armor.Handle(true);
                return;
            }
        }
    }
}
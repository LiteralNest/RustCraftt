using System.Collections.Generic;
using Animation_System;
using PlayerDeathSystem.ArmorsSystem;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerCorpDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterCorpesAnimator _characterAnimationsHandler;
        [SerializeField] private CorpesArmorsContainer _corpesArmorsContainer;

        public void Init()
        {
            _characterAnimationsHandler.DisplayDeathServerRpc();
        }

        public void DisplayCloth(List<int> items)
        {
            _corpesArmorsContainer.ClearArmors();
            foreach(var item in items)
                _corpesArmorsContainer.AssignItem(item);
        }
    }
}
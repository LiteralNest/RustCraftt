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

        public void Init(List<int> items)
        {
            _characterAnimationsHandler.DisplayDeathServerRpc();
            foreach(var item in items)
                _corpesArmorsContainer.AssignItemServerRpc(item);
        }
    }
}
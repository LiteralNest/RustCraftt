using Animation_System;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerCorpDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterCorpesAnimator _characterAnimationsHandler;

        public void Init()
        {
            _characterAnimationsHandler.DisplayDeathServerRpc();
        }
    }
}
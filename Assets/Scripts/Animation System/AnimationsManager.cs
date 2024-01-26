using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Animation_System
{
    public class AnimationsManager : NetworkBehaviour
    {
        public static AnimationsManager Singleton { get; private set; }

        private NetworkVariable<int> CurrentAnimationId =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> CurrentInventoryAnimationId =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public CharacterAnimationsHandler CharacterAnimationsHandler { get; set; }
        public CharacterAnimationsHandler InventoryAnimationsHandler { get; set; }

        private async void Start()
        {
            await Task.Delay(1000);
            CurrentAnimationId.OnValueChanged += (int prevValue, int newValue) =>
            {
                
            };
            CurrentInventoryAnimationId.OnValueChanged += (int prevValue, int newValue) =>
            {
                
            };
            if (!IsOwner) return;
            Singleton = this;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetAnimationServerRpc(int value, bool isMainPlayer = true)
        {
            if (!IsServer) return;
            if (isMainPlayer)
            {
                CurrentAnimationId.Value = value;
            }
            else
            {
                CurrentInventoryAnimationId.Value = value;
            }
            SetAnimationClientRpc(value, isMainPlayer);
        }

        [ClientRpc]
        private void SetAnimationClientRpc(int value, bool isMainPlayer = true)
        {
            if (isMainPlayer)
            {
                if(CharacterAnimationsHandler)
                    CharacterAnimationsHandler.SetAnimation(value);
            }
            else
            {
                if(InventoryAnimationsHandler)
                    InventoryAnimationsHandler.SetAnimation(value);
            }
        }

        private void SetAnimation(string value, bool isMainPlayer = true)
        {
            if (CharacterAnimationsHandler == null) return;
            SetAnimationServerRpc(CharacterAnimationsHandler.GetAnimationNum(value), isMainPlayer);
        }
        
        [ContextMenu("Set Sit")]
        public void SetSit()
        {
            SetAnimation("Sit");
        }

        public void SetStand()
        {
            SetAnimation("Stand");
        }

        [ContextMenu("Set Walk")]
        public void SetWalk()
        {
            SetAnimation("Walk");
        }

        [ContextMenu("Set Idle")]
        public void SetIdle()
        {
            SetAnimation("Idle");
        }

        [ContextMenu("Set Swim")]
        public void SetSwim()
        {
            SetAnimation("Swim");
        }

        [ContextMenu("Jump")]
        public void SetJump()
            => SetAnimation("Jump");

        [ContextMenu("Knock Down")]
        public void SetKnockDown()
            => SetAnimation("KnockDown");

        [ContextMenu("Fall")]
        public void SetFall()
            => SetAnimation("Fall");

        [ContextMenu("Death")]
        public void SetDeath()
            => SetAnimation("Dead", false);
        
        [ContextMenu("StartCrouch")]
        public void SetStartCrouching()
            => SetAnimation("StartCrouch");
        
        public void SetCrouch()
            => SetAnimation("Crouch");
        
        public void SetStopCrouching()
            => SetAnimation("StopCrouch");
    }
}
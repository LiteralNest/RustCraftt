using System.Threading.Tasks;
using PlayerDeathSystem;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace Character_Stats
{
    public class CharacterHpHandler : NetworkBehaviour
    {
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private CameraShake _cameraShake;

        private NetworkVariable<int> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public NetworkVariable<bool> WasKnockedDown { get; set; }= new(false, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);
        public int Hp => _currentHp.Value;


        private async void Start()
        {
            await Task.Delay(1000);
            if (IsOwner)
            {
                _characterStats.AssignHp(_currentHp.Value);
                _currentHp.OnValueChanged += (int prevValue, int newValue) => DisplayDamage();
            }
        }
        private void DisplayDamage()
        {
            if (_characterStats == null) return;
            if (!IsOwner) return;
            _characterStats.DisplayHp(_currentHp.Value);
            if(Hp <= 0)
                PlayerKiller.Singleton.DieServerRpc(UserDataHandler.singleton.UserData.Id, false);
            else if (Hp <= 5)
                PlayerKnockDowner.Singleton.KnockDownServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void AssignHpServerRpc(int value)
        {
            if (!IsServer) return;
            _currentHp.Value = value;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetKnockedDownServerRpc(bool value)
        {
            WasKnockedDown.Value = value;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void GetDamageServerRpc(int damageAmount)
        {
            if (!IsServer) return;
            if (!WasKnockedDown.Value)
            {
                if (_currentHp.Value - damageAmount < 5)
                {
                    _currentHp.Value = 5; 
                    WasKnockedDown.Value = true;
                }
                else
                    _currentHp.Value -= damageAmount;
            }
            else
            {
                if(_currentHp.Value - damageAmount < 0)
                    _currentHp.Value = 0;
                else
                    _currentHp.Value -= damageAmount;
            }
        }

        public void Shake()
        {
            if (!_cameraShake) return;
            _cameraShake.StartShake(0.5f, 0.1f);
        }
    }
}
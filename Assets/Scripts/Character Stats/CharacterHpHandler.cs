using System.Threading.Tasks;
using PlayerDeathSystem;
using Unity.Netcode;
using UnityEngine;

namespace Character_Stats
{
    public class CharacterHpHandler : NetworkBehaviour
    {
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private CameraShake _cameraShake;

        private NetworkVariable<int> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);
        
        public ushort Hp => (ushort)_currentHp.Value;

        private async void Start()
        {
            await Task.Delay(1000);
            if (IsOwner)
            {
                _currentHp.Value = 100;
                _characterStats.AssignHp(_currentHp.Value);
                _currentHp.OnValueChanged += (int prevValue, int newValue) => DisplayDamage();
            }
        }
        

        private void DisplayDamage()
        {
            if (_characterStats == null) return;
            if(!IsOwner) return;
            _characterStats.DisplayHp(_currentHp.Value);
            if(Hp <= 0)
                PlayerKnockDowner.Singleton.KnockDownServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void AssignHpServerRpc(int value)
        {
            if (!IsServer) return;
            _currentHp.Value = value;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void GetDamageServerRpc(int damageAmount)
        {
            if (!IsServer) return;
            Debug.Log("Getted damage: " + damageAmount);
            _currentHp.Value -= damageAmount;
        }
        
        public void Shake()
        {
            if (!_cameraShake) return;
            _cameraShake.StartShake(0.5f, 0.1f);
        }
    }
}
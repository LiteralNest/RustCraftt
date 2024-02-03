using System.Threading.Tasks;
using Events;
using FightSystem.Weapon.Explosive;
using PlayerDeathSystem;
using Sound_System.FightSystem.Damage;
using UI;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Character_Stats
{
    public class CharacterHpHandler : MonoBehaviour, IDamagable
    {
        [SerializeField] private CameraShake _cameraShake;

        private int _currentHp;

        public NetworkVariable<bool> WasKnockedDown { get; set; } = new(false, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public int Hp => _currentHp;


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
            GlobalEventsContainer.CharacterHpChanged?.Invoke();
            _characterStats.DisplayHp(_currentHp.Value);
            if (Hp <= 0)
            {
                MainUiHandler.Singleton.DisplayKnockDownScreen(false);
                PlayerKiller.Singleton.DieServerRpc(UserDataHandler.Singleton.UserData.Id, false);
            }
            else if (Hp <= 5)
                PlayerKnockDowner.Singleton.KnockDownServerRpc(UserDataHandler.Singleton.UserData.Id);
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
                if (_currentHp.Value - damageAmount < 0)
                    _currentHp.Value = 0;
                else
                    _currentHp.Value -= damageAmount;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddHpServerRpc(int value)
        {
            if(!IsServer) return;
            _currentHp.Value += value;
            if(_currentHp.Value > 100)
                _currentHp.Value = 100;
        }

        #region Damagable

        public AudioClip GetPlayerDamageClip()
        {
            return null;
        }

        public int GetHp()
            => _currentHp.Value;

        public int GetMaxHp()
            => 0;

        public void GetDamage(int damage, bool playSound = true)
            => _currentHp.Value -= damage;

        public void Destroy()
        {
        }
        

        #endregion
    }
}
using FightSystem.Damage;
using InteractSystem;
using Player_Controller;
using Sound_System.FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Placing_Objects
{
    public class PlacingObjectDamageHandler : NetworkBehaviour, IDamagable, IRayCastHpDusplayer
    {
        [SerializeField] private int _maxHp = 100;
        private NetworkVariable<int> _hp = new();
        [SerializeField] private AudioClip _playerHitSound;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _hp.Value = _maxHp;
        }

        public AudioClip GetPlayerDamageClip()
            => _playerHitSound;

        public int GetHp()
            => _hp.Value;

        public int GetMaxHp()
            => _maxHp;

        public void GetDamage(int damage)
            => GetDamageServerRpc(damage);

        public void Destroy()
        {
            GetComponent<NetworkObject>().Despawn();
        }

        public void Shake()
        {
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damage)
        {
            _hp.Value -= damage;
            if (_hp.Value <= 0)
                Destroy();
        }

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayObjectHp(this);
    }
}
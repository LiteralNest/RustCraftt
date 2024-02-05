using FightSystem.Damage;
using InteractSystem;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Placing_Objects
{
    public class PlacingObjectDamageHandler : Building, IBuildingDamagable, IRayCastHpDusplayer
    {
        [SerializeField] private int _maxHp = 100;
        private NetworkVariable<float> _hp = new();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _hp.Value = _maxHp;
        }

        public int GetHp()
            => (int)_hp.Value;

        public int GetMaxHp()
            => _maxHp;

        public void GetDamageOnServer(int damage)
            => GetDamageServerRpc(damage);

        public void Destroy()
        {
            GetComponent<NetworkObject>().Despawn();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damageItemId)
        {
            var damage = GetDamageAmount(damageItemId);
            _hp.Value -= damage;
            if (_hp.Value <= 0)
                Destroy();
        }

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayBuildingHp(this);
    }
}
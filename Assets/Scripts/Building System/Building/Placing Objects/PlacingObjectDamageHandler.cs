using FightSystem.Damage;
using InteractSystem;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Placing_Objects
{
    public class PlacingObjectDamageHandler : Building, IBuildingDamagable, IRayCastHpDusplayer
    {
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private int _maxHp = 100;
        private NetworkVariable<float> _hp = new();

        private void Awake()
        {
            if (_networkObject == null)
                _networkObject = GetComponent<NetworkObject>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _hp.Value = _maxHp;
        }

        public void GetDamageByExplosive(int explosiveId, float distance, float radius)
        {
            var damage = GetDamageAmountByExplosive(explosiveId, distance, radius);
            GetDamageServerRpc((int)damage);
        }

        public int GetHp()
            => (int)_hp.Value;

        public int GetMaxHp()
            => _maxHp;

        public void GetDamageOnServer(int itemId)
            => GetDamageServerRpcByIdServerRpc(itemId);

        public void Destroy()
            => _networkObject.Despawn();

        [ServerRpc(RequireOwnership =  false)]
        private void GetDamageServerRpc(int damage)
        {
            _hp.Value -= damage;
            if (_hp.Value <= 0)
                Destroy();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpcByIdServerRpc(int damageItemId)
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
using CloudStorageSystem;
using FightSystem.Damage;
using InteractSystem;
using Player_Controller;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Placing_Objects
{
    public class PlacingObjectDamageHandler : Building, IBuildingDamagable, IRayCastHpDisplayer
    {
        [SerializeField] private DropableStorage _targetBag;
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
            if (IsServer)
            {
                _hp.OnValueChanged += (float prevValue, float newValue) =>
                {
                    CloudSaveEventsContainer.OnStructureHpChanged?.Invoke((int)_hp.Value, transform.position);
                };
                _hp.Value = _maxHp;
            }
        }

        public void GetDamageByExplosive(int explosiveId, float distance, float radius)
        {
            var damage = GetDamageAmountByExplosive(explosiveId, distance, radius);
            _hp.Value -= damage;
            if (_hp.Value <= 0)
                Destroy();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damageItemId)
        {
            if (!IsServer) return;
            GetDamageOnServer(damageItemId);
        }

        public void GetDamageToServer(int damageItemId)
            => GetDamageServerRpc(damageItemId);

        public int GetHp()
            => (int)_hp.Value;

        public int GetMaxHp()
            => _maxHp;

        public void GetDamageOnServer(int itemId)
        {
            if (!IsServer) return;
            var damage = GetDamageAmount(itemId);
            _hp.Value -= damage;
            if (_hp.Value <= 0)
                Destroy();
        }

        public void Destroy()
        {
            if (_targetBag)
                if (_targetBag.TryDisplayBagOnServer())
                    return;
            CloudSaveEventsContainer.OnStructureDestroyed?.Invoke(transform.position);
            _networkObject.Despawn();
        }

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayBuildingHp(this);
    }
}
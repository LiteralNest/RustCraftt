using FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Placing_Objects
{
    public class PlacingObjectDamageHandler : NetworkBehaviour, IDamagable
    {
        [SerializeField] private int _maxHp = 100;
        private NetworkVariable<int> _hp = new();
 
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            gameObject.tag = "DamagingItem";
            _hp.Value = _maxHp;
        }
        
        public int GetHp()
            => _hp.Value;

        public int GetMaxHp()
            => _maxHp;

        public void GetDamage(int damage, bool playSound = true)
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
    }
}
using AI.Animals;
using FightSystem.Damage;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace AI
{
    public class AIStats : NetworkBehaviour, IDamagable
    {
        [SerializeField] private AnimalID _animalId;
        
        [SerializeField] private NetworkVariable<ushort> _hp = new NetworkVariable<ushort>(100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        private int _maxHp;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _maxHp = _hp.Value;
        }

        #region IDamagable

        public int GetHp()
            => _hp.Value;

        public int GetMaxHp()
            => _maxHp;

        [ServerRpc(RequireOwnership = false)]
        private void SetHpServerRpc(ushort hp)
        {
            if(!IsServer) return;
            _hp.Value = hp;
            if (hp <= 0)
                Destroy();
        }

        public void GetDamage(int damage, bool playSound = true)
        {
            int currHp = _hp.Value;
            var newHp = currHp - damage;
            if (newHp < 0) newHp = 0;
            SetHpServerRpc((ushort)newHp);
        }


        public void Destroy()
        {
            AnimalObjectInstantiator.singleton.InstantiateAnimalObjectServerRpc(_animalId.Id, transform.position, transform.rotation.eulerAngles);
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }

        public void Shake()
        {
            
        }

        #endregion
    }
}
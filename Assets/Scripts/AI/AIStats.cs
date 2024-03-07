using AI.Animals;
using FightSystem.Damage;
using InteractSystem;
using Multiplayer;
using Player_Controller;
using Sound_System;
using Unity.Netcode;
using UnityEngine;

namespace AI
{
    public class AIStats : NetworkBehaviour, IDamagable, IRayCastHpDisplayer
    {
        [SerializeField] private Transform _corpSpawnPos;
        [SerializeField] private AnimalID _animalId;

        [SerializeField] private NetworkVariable<ushort> _hp = new NetworkVariable<ushort>(100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        private bool _destroyed;
        private int _maxHp;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _maxHp = _hp.Value;
        }

        #region IDamagable

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damage)
        {
            if (!IsServer) return;
            GetDamageOnServer(damage);
        }

        public void GetDamageToServer(int damage)
            => GetDamageServerRpc(damage);

        public AudioClip GetPlayerDamageClip()
            => GlobalSoundsContainer.Singleton.HitSound;

        public int GetHp()
            => _hp.Value;

        public int GetMaxHp()
            => _maxHp;


        public void GetDamageOnServer(int damage)
        {
            if (!IsServer || _destroyed) return;
            int currHp = _hp.Value;
            var newHp = currHp - damage;
            if (newHp < 0) newHp = 0;
            _hp.Value = (ushort)newHp;
            if (_hp.Value <= 0)
                Destroy();
        }
        
        public void Destroy()
        {
            _destroyed = true;
            AnimalObjectInstantiator.singleton.SpawnAnimalCorpById(_animalId.Id, _corpSpawnPos.position,
                _corpSpawnPos.rotation.eulerAngles);
            GetComponent<NetworkObject>().Despawn();
        }

        [ContextMenu("Die")]
        public void DieTest()
        {
            GetDamageServerRpc(_hp.Value);
        }
        
        #endregion

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayObjectHp(this);
    }
}
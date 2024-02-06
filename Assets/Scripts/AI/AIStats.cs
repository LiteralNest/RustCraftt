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

        public AudioClip GetPlayerDamageClip()
            => GlobalSoundsContainer.Singleton.HitSound;

        public int GetHp()
            => _hp.Value;

        public int GetMaxHp()
            => _maxHp;


        public void GetDamageOnServer(int damage)
        {
            if (!IsServer) return;
            int currHp = _hp.Value;
            var newHp = currHp - damage;
            if (newHp < 0) newHp = 0;
            _hp.Value = (ushort)newHp;
            if (_hp.Value <= 0)
                Destroy();
        }


        public void Destroy()
        {
            AnimalObjectInstantiator.singleton.InstantiateAnimalObjectServerRpc(_animalId.Id, transform.position,
                transform.rotation.eulerAngles);
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }

        public void Shake()
        {
        }

        #endregion

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayObjectHp(this);
    }
}
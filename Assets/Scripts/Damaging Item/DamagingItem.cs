using System.Collections;
using System.Collections.Generic;
using FightSystem.Damage;
using Multiplayer;
using Sound_System;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Damaging_Item
{
    [RequireComponent(typeof(NetworkSoundPlayer))]
    [RequireComponent(typeof(NetworkObject))]
    public class DamagingItem : NetworkBehaviour, IDamagable
    {
        
        [SerializeField] private NetworkVariable<int> _currentHp = new(50, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private float _spawningYOffset = 1f;
        [SerializeField] private List<LootCell> _loot = new List<LootCell>();
        [SerializeField] private List<GameObject> _renderingObjects = new List<GameObject>();
        [SerializeField] private List<Collider> _colliders = new List<Collider>();
        [SerializeField] private float _recoveringTime = 60;
        [Header("Sound")] [SerializeField] private AudioClip _damagingSound;
        [SerializeField] private AudioClip _destroyingSound;


        private NetworkSoundPlayer _soundPlayer;
        private NetworkObject _networkObject;

        private int _cachedHp;

        private void Start()
        {
            _networkObject = GetComponent<NetworkObject>();
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            transform.tag = "DamagingItem";
            _cachedHp = _currentHp.Value;
        }

        private void SpawnLootCell(LootCell cell)
        {
            int rand = Random.Range(cell.MinimalCount, cell.MaximalCount);
            var fixedPos = transform.position;
            fixedPos.y += _spawningYOffset;
            InstantiatingItemsPool.sigleton.SpawnObjectOnServer(
                new CustomSendingInventoryDataCell(cell.Item.Id, rand, 100, 0), fixedPos);
        }

        public void Destroy()
            => StartCoroutine(DestroyRoutine());

        public void Shake()
        {
            throw new System.NotImplementedException();
        }

        public AudioClip GetPlayerDamageClip()
            => GlobalSoundsContainer.Singleton.HitSound;

        public int GetHp()
            => (ushort)_currentHp.Value;

        public int GetMaxHp()
            => _cachedHp;

        public void GetDamage(int damage, bool playSound = true)
            => GetDamageServerRpc(damage, playSound);

        private void HandleRenderers(bool value)
        {
            foreach (var renderer in _renderingObjects)
                renderer.SetActive(value);
            foreach (var collider in _colliders)
                collider.enabled = value;
        }

        private IEnumerator DestroyRoutine()
        {
            if (!IsServer) yield break;
            _soundPlayer.PlayOneShot(_destroyingSound);
            foreach(var collider in _colliders)
                collider.enabled = false;
            TurnRendederersClientRpc(false);
            foreach (var cell in _loot)
                SpawnLootCell(cell);
            StartCoroutine(RecoverRoutine());
        }

        private IEnumerator RecoverRoutine()
        {
            if(!IsServer) yield break;
            yield return new WaitForSeconds(_recoveringTime);
            _currentHp.Value = _cachedHp;
            foreach(var collider in _colliders)
                collider.enabled = true;
            TurnRendederersClientRpc(true);
        }

        private void CheckHp(int value)
        {
            if (!IsServer) return;

            if (value > 0) return;
            Destroy();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDamageServerRpc(int damage, bool value)
        {
            _currentHp.Value -= damage;
            CheckHp(_currentHp.Value);
            _soundPlayer.PlayOneShot(_damagingSound);
        }

        [ClientRpc]
        private void TurnRendederersClientRpc(bool value)
            => HandleRenderers(value);
    }
}
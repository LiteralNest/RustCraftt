using System.Collections;
using System.Collections.Generic;
using FightSystem.Damage;
using InteractSystem;
using Multiplayer;
using Player_Controller;
using Sound_System;
using Storage_System;
using Storage_System.Loot_Boxes_System;
using Unity.Netcode;
using UnityEngine;

namespace Damaging_Item
{
    [RequireComponent(typeof(NetworkSoundPlayer))]
    [RequireComponent(typeof(NetworkObject))]
    public class DamagingItem : NetworkBehaviour, IDamagable, IRayCastHpDisplayer
    {
        [SerializeField] private NetworkVariable<int> _currentHp = new(50, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private LootBoxSlot _scrapSlot;
        [SerializeField] private List<LootBoxSlot> _lootCells = new List<LootBoxSlot>();
        [SerializeField] private float _spawningYOffset = 1f;
        [SerializeField] private List<GameObject> _renderingObjects = new List<GameObject>();
        [SerializeField] private List<Collider> _colliders = new List<Collider>();
        [SerializeField] private float _recoveringTime = 60;
        [Header("Sound")] [SerializeField] private AudioClip _damagingSound;
        [SerializeField] private AudioClip _destroyingSound;

        private NetworkSoundPlayer _soundPlayer;
        private int _cachedHp;

        private void Start()
        {
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            transform.tag = "DamagingItem";
            _cachedHp = _currentHp.Value;
        }

        private void SpawnCell(LootBoxSlot slot)
        {
            var fixedPos = transform.position;
            fixedPos.y += _spawningYOffset;
            InstantiatingItemsPool.sigleton.SpawnObjectOnServer(
                new CustomSendingInventoryDataCell(slot.Item.Id, Random.Range(slot.RandCount.x, slot.RandCount.y + 1),
                    100, 0), fixedPos);
        }

        [ContextMenu("Generate Cells")]
        private void GenerateCells()
        {
            SpawnCell(_scrapSlot);
            foreach (var set in _lootCells)
            {
                var rand = Random.Range(0, 100);
                if (rand > set.Chance) continue;
                SpawnCell(set);
                return;
            }
            SpawnCell(_lootCells[0]);
        }

        public void Destroy()
            => StartCoroutine(DestroyRoutine());


        public AudioClip GetPlayerDamageClip()
            => GlobalSoundsContainer.Singleton.HitSound;

        public int GetHp()
            => (ushort)_currentHp.Value;

        public int GetMaxHp()
            => _cachedHp;

        public void GetDamageOnServer(int damage)
        {
            if (!IsServer) return;
            if (_currentHp.Value <= 0) return;
            _currentHp.Value -= damage;
            CheckHp(_currentHp.Value);
            _soundPlayer.PlayOneShot(_damagingSound);
        }

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
            foreach (var collider in _colliders)
                collider.enabled = false;
            TurnRendederersClientRpc(false);
            GenerateCells();
            StartCoroutine(RecoverRoutine());
        }

        private IEnumerator RecoverRoutine()
        {
            if (!IsServer) yield break;
            yield return new WaitForSeconds(_recoveringTime);
            _currentHp.Value = _cachedHp;
            foreach (var collider in _colliders)
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
        private void GetDamageServerRpc(int damage)
        {
            if (!IsServer) return;
            GetDamageOnServer(damage);
        }

        public void GetDamageToServer(int damage)
            => GetDamageServerRpc(damage);
        
        [ClientRpc]
        private void TurnRendederersClientRpc(bool value)
            => HandleRenderers(value);

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayObjectHp(this);
    }
}
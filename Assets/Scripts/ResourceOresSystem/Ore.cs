using System.Collections;
using System.Collections.Generic;
using Events;
using Items_System.Ore_Type;
using Sirenix.OdinInspector;
using TerrainTools;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    public class Ore : NetworkBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private GatheringOreAnimator _animator;

        [Header("Start init")] [SerializeField]
        protected int _hp = 100;

        [SerializeField] protected NetworkVariable<int> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        
        [SerializeField] protected List<OreSlot> _resourceSlots = new List<OreSlot>();
        [SerializeField] private GameObject _vfxEffect;

        private OreObjectsPlacer _objectsPlacer;

        public bool Recovering { get; protected set; } = false;

        protected virtual void Start()
            => _animator.SetIdle();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                _currentHp.Value = _hp;

            base.OnNetworkSpawn();
        }

        public void Init(OreObjectsPlacer objectsPlacer)
            => _objectsPlacer = objectsPlacer;

        [Button]
        public void Destroy()
        {
            StartCoroutine(DestroyRoutine());
        }
        
        protected void AddResourcesToInventory()
        {
            foreach (var slot in _resourceSlots)
            {
                var rand = Random.Range(slot.CountRange.x, slot.CountRange.y + 1);
                GlobalEventsContainer.OnInventoryItemAdded?.Invoke(new InventoryCell(slot.Resource, rand));
                InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(slot.Resource.Id, rand, 0);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        protected void MinusHpServerRpc()
        {
            if (_currentHp.Value <= 0)
            {
                StartCoroutine(DestroyRoutine());
                return;
            }
            _currentHp.Value--;
        }
        
        private IEnumerator DestroyRoutine()
        {
            yield return _animator.SetFallRoutine();
            StartCoroutine(_objectsPlacer.RegenerateObjectRoutine(this));
        }

        [ClientRpc]
        private void DisplayVfxClientRpc(Vector3 pos, Vector3 rot)
        {
            Instantiate(_vfxEffect, pos, Quaternion.FromToRotation(Vector3.up, rot));
        }

        [ServerRpc(RequireOwnership = false)]
        protected void DisplayVfxServerRpc(Vector3 pos, Vector3 rot)
        {
            if (!IsServer) return;
            DisplayVfxClientRpc(pos, rot);
        }
    }
}
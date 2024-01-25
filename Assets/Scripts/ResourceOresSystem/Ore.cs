using System.Collections;
using System.Collections.Generic;
using Events;
using Items_System.Items;
using Items_System.Ore_Type;
using Sirenix.OdinInspector;
using TerrainTools;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    public abstract class Ore : NetworkBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private GatheringOreAnimator _animator;

        [Header("Start init")] [SerializeField]
        protected NetworkVariable<int> _currentHp = new(100);

        [SerializeField] protected List<OreSlot> _resourceSlots = new List<OreSlot>();
        [SerializeField] private GameObject _vfxEffect;

        private OreObjectsPlacer _objectsPlacer;

        public NetworkVariable<int> CurrentHp => _currentHp;

        public bool Recovering { get; protected set; } = false;

        public void Init(OreObjectsPlacer objectsPlacer)
            => _objectsPlacer = objectsPlacer;

        protected void AddResourcesToInventory()
        {
            foreach (var slot in _resourceSlots)
            {
                int rand = Random.Range(slot.CountRange.x, slot.CountRange.y + 1);
                GlobalEventsContainer.OnInventoryItemAdded?.Invoke(new InventoryCell(slot.Resource, rand));
                InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(slot.Resource.Id, rand, 0);
            }
        }

        public virtual bool CanUseTool(Tool targetTool)
            => true;

        protected void AddResourcesToInventory(Tool targetTool, OreToolsForGatheringSlots toolSlot)
        {
            foreach (var slot in _resourceSlots)
            {
                int rand = 0;
                if (slot.ShouldAddWithRand || targetTool == null)
                    rand = Random.Range(slot.CountRange.x, slot.CountRange.y + 1);
                else
                    rand = targetTool.GatheringAmount * ((100 - toolSlot.LossAmount) / 100);

                if (rand <= 0) rand = 1;

                GlobalEventsContainer.OnInventoryItemAdded?.Invoke(new InventoryCell(slot.Resource, rand));
                InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(slot.Resource.Id, rand, 0);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        protected void MinusHpServerRpc()
        {
            _currentHp.Value--;
            if (_currentHp.Value <= 0)
            {
                StartCoroutine(DestroyRoutine());
                return;
            }
        }

        private IEnumerator DestroyRoutine()
        {
            yield return _animator.SetFallRoutine();
            StartCoroutine(_objectsPlacer.RegenerateObjectRoutine(this));
        }

        [ClientRpc]
        private void DisplayVfxClientRpc(Vector3 pos, Vector3 rot)
        {
            if (!_vfxEffect) return;
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
using System.Collections;
using System.Collections.Generic;
using Inventory_System;
using Items_System.Items;
using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    public class ResourceOre : Ore
    {
        [Header("Attached Components")] [SerializeField]
        private NetworkObject _targetNetworkObject;
        [SerializeField] private List<Collider> _colliders;

        [Header("VFX")] 
        [SerializeField] private GameObject _vfxEffect;
        [SerializeField] private Transform _vfxParrent;

        [Header("Main Parameters")] 
        [SerializeField] private List<OreToolsForGatheringSlots> _toolsForGathering = new List<OreToolsForGatheringSlots>();
        [field: SerializeField] public AudioClip GatheringClip { get; private set; }
        [SerializeField] private bool _shouldDestroy = true;
        [SerializeField] private GatheringOreAnimator _animator;

        private void Awake()
        {
            if(_targetNetworkObject == null)
                _targetNetworkObject = GetComponent<NetworkObject>();
        }
        
        private bool SlotFound(Item item, out OreToolsForGatheringSlots slot)
        {
            foreach (var tool in _toolsForGathering)
            {
                if (tool.Tool.Id != item.Id) continue;
                slot = tool;
                return true;
            }

            slot = default;
            return false;
        }

        public override bool CanUseTool(Tool tool)
            => SlotFound(tool, out OreToolsForGatheringSlots toolSlot);

        public void MinusHp(Item targetTool, out bool destroyed, Vector3 lastRayPos, Vector3 lastRayRot)
        {
            destroyed = false;
            if (_currentHp.Value <= 0) return;
            if (!SlotFound(targetTool, out OreToolsForGatheringSlots toolSlot)) return;
            AddResourcesToInventory(targetTool as Tool, toolSlot);
            if (_vfxEffect != null)
                DisplayVfxServerRpc(lastRayPos, lastRayRot);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentHp(1);
            MinusHpServerRpc();
            destroyed = _currentHp.Value <= 0;
        }

        [ClientRpc]
        private void DisplayVfxClientRpc(Vector3 pos, Vector3 rot)
        {
            if (!_vfxEffect) return;
            Instantiate(_vfxEffect, pos, Quaternion.FromToRotation(Vector3.up, rot), _vfxParrent);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DisplayVfxServerRpc(Vector3 pos, Vector3 rot)
        {
            if (!IsServer) return;
            DisplayVfxClientRpc(pos, rot);
        }

        [ClientRpc]
        private void TurnCollidersClientRpc(bool value)
        {
            foreach (var collider in _colliders)
                collider.enabled = value;
        }

        protected override IEnumerator DestroyRoutine()
        {
            base.DoAfterDestroying();
            TurnCollidersClientRpc(false);
            if (_animator)
                yield return _animator.SetFallRoutine();
            if (ObjectsPlacer)
                StartCoroutine(ObjectsPlacer.RegenerateObjectRoutine(this));
            else if(_shouldDestroy)
                _targetNetworkObject.Despawn();
        }
    }
}
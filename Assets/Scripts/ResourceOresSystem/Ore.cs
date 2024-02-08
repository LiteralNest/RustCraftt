using System.Collections;
using System.Collections.Generic;
using Events;
using Inventory_System;
using Items_System.Items;
using TerrainTools;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    public abstract class Ore : NetworkBehaviour
    {
        [Header("Start init")] [SerializeField]
        protected List<OreSlot> _resourceSlots = new List<OreSlot>();

        protected OreObjectsPlacer ObjectsPlacer;
        public NetworkVariable<int> CurrentHp => _currentHp;
        [SerializeField] protected NetworkVariable<int> _currentHp = new(20);

        protected int CachedMaxHp;

        private void Awake()
            => CachedMaxHp = _currentHp.Value;

        public void Init(OreObjectsPlacer objectsPlacer)
            => ObjectsPlacer = objectsPlacer;

        protected void AddResourcesToInventory()
        {
            foreach (var slot in _resourceSlots)
            {
                int rand = Random.Range(slot.CountRange.x, slot.CountRange.y + 1);
                InventoryHandler.singleton.CharacterInventory.AddItemToSlotWithAlert(slot.Resource.Id, rand, 0);
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

                InventoryHandler.singleton.CharacterInventory.AddItemToSlotWithAlert(slot.Resource.Id, rand, 0);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        protected void MinusHpServerRpc()
        {
            _currentHp.Value--;
            if (_currentHp.Value <= 0)
                StartCoroutine(DestroyRoutine());
        }


        protected virtual void DoAfterDestroying()
        {
        }

        protected virtual IEnumerator DestroyRoutine()
        {
            yield break;
        }
    }
}
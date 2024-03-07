using System.Collections;
using System.Collections.Generic;
using Inventory_System;
using Items_System.Items.Abstract;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootBox : Storage
    {
        [Header("Main Values")] [SerializeField]
        private LootBoxSlot _scrap;

        [SerializeField] protected List<LootBoxSlot> _setsPool = new List<LootBoxSlot>();
        [SerializeField] private float _recoverTime = 120f;

        [Header("Display")] [SerializeField] private List<Collider> _colliders;
        [SerializeField] private List<Renderer> _rendereres;
        [SerializeField] private GameObject _canvas;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            ItemsNetData.OnValueChanged += (oldValue, newValue) =>
            {
                TryResetPanels();
            };
            if (!IsServer) return;
            GenerateCells();
            ItemsNetData.OnValueChanged += (oldValue, newValue) => CheckCells();
        }

        public void TryResetPanels()
        {
            if (!Opened) return;
            if (!StorageEmpty()) return;
            _canvas.SetActive(false);
            InventoryHandler.singleton.InventoryPanelsDisplayer.HandleCharacterPreview(true);
            PlayerNetCode.Singleton.ActiveInvetoriesHandler.AddActiveInventory(null);
            PlayerNetCode.Singleton.ItemInfoHandler.ResetPanel();
        }

        public override int GetAvailableCellIndexForMovingItem(Item item)
            => -1;

        protected bool StorageEmpty()
        {
            var cells = ItemsNetData.Value.Cells;
            foreach (var cell in cells)
                if (cell.Id != -1)
                    return false;
            return true;
        }

        protected virtual void CheckCells()
        {
            if (!StorageEmpty()) return;
            StartCoroutine(RecoverRoutine());
        }

        [ClientRpc]
        private void TurnRenderersClientRpc(bool value)
        {
            foreach (var renderer in _rendereres)
                renderer.enabled = value;
            foreach (var collider in _colliders)
                collider.enabled = value;
        }

        private IEnumerator RecoverRoutine()
        {
            TurnRenderersClientRpc(false);
            yield return new WaitForSeconds(_recoverTime);
            TurnRenderersClientRpc(true);
            GenerateCells();
        }

        protected LootBoxSlot GetRandomSlot(List<LootBoxSlot> setsPool)
        {
            var rand = Random.Range(0, 100);
            foreach (var slot in setsPool)
                if (slot.Chance > rand)
                    return slot;
            return setsPool[0];
        }

        [ContextMenu("Generate Cells")]
        protected virtual void GenerateCells()
        {
            AddItemToDesiredSlot(_scrap.Item.Id, Random.Range(_scrap.RandCount.x, _scrap.RandCount.y + 1), 0);
            var set = GetRandomSlot(_setsPool);
            AddItemToDesiredSlot(set.Item.Id, Random.Range(set.RandCount.x, set.RandCount.y + 1), 0);
        }
    }
}
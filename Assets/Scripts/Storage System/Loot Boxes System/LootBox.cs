using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Storage_System.Loot_Boxes_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class LootBox : Storage
    {
        [Header("Main Values")] [SerializeField]
        private LootBoxSlot _scrap;

        [SerializeField] private List<LootBoxSlot> _setsPool = new List<LootBoxSlot>();
        [SerializeField] private float _recoverTime = 120f;

        [Header("Display")] [SerializeField] private List<Collider> _colliders;
        [SerializeField] private List<Renderer> _rendereres;
        [SerializeField] private GameObject _canvas;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsServer) return;
            GenerateCells();
            ItemsNetData.OnValueChanged += (oldValue, newValue) => CheckCells();
        }

        private void CheckCells()
        {
            var cells = ItemsNetData.Value.Cells;
            foreach (var cell in cells)
                if (cell.Id != -1)
                    return;
            StartCoroutine(RecoverRoutine());
        }

        private void TurnRenderers(bool value)
        {
            foreach (var renderer in _rendereres)
                renderer.enabled = value;
            foreach (var collider in _colliders)
                collider.enabled = value;
            if (!value)
            {
                _canvas.SetActive(false);
                InventoryHandler.singleton.InventoryPanelsDisplayer.HandleCharacterPreview(true);
            }
               
        }

        private IEnumerator RecoverRoutine()
        {
            TurnRenderers(false);
            yield return new WaitForSeconds(_recoverTime);
            TurnRenderers(true);
            GenerateCells();
        }

        [ContextMenu("Generate Cells")]
        private void GenerateCells()
        {
            AddItemToDesiredSlot(_scrap.Item.Id, Random.Range(_scrap.RandCount.x, _scrap.RandCount.y + 1), 0);
            foreach (var set in _setsPool)
            {
                var rand = Random.Range(0, 100);
                if (rand > set.Chance) continue;
                AddItemToDesiredSlot(set.Item.Id, Random.Range(set.RandCount.x, set.RandCount.y + 1), 0);
                return;
            }
        }
    }
}
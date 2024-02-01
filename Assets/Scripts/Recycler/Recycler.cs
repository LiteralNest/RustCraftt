using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory_System;
using Items_System.Items.Recycling_Item;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Recycler
{
    public class Recycler : Storage
    {
        [HideInInspector] public NetworkVariable<bool> Turned = new NetworkVariable<bool>(false);

        [SerializeField] private Animator _animator;
        [SerializeField] private float _recyclingTime = 1;
        [SerializeField] private AudioSource _source;

        [Header("UI")] [SerializeField] private GameObject _turnOnPanel;
        [SerializeField] private GameObject _turnOffPanel;

        private bool _recycling;

        private void Start()
            => gameObject.tag = "Recycler";

        public override void OnNetworkSpawn()
        {
            Turned.OnValueChanged += (bool prevValue, bool newValue) => { Turn(newValue); };
            Turn(Turned.Value);
            base.OnNetworkSpawn();
        }

        private void Update()
        {
            if (!Turned.Value) return;
            TryRecycle();
        }

        private void Turn(bool value)
        {
            _turnOnPanel.SetActive(!value);
            _turnOffPanel.SetActive(value);
            if (value)
            {
                _animator.SetTrigger("Work");
                _source.Play();
            }
            else
            {
                _animator.SetTrigger("Idle");
                _source.Stop();
            }
        }

        private async void RecycleItem(RecyclingItem item, int slotIndex)
        {
            await Task.Delay((int)(_recyclingTime * 1000));
            if (!_recycling) return;
            List<CustomSendingInventoryDataCell> recyclingCells = new List<CustomSendingInventoryDataCell>();
            for (int i = 5; i < 10; i++)
            {
                recyclingCells.Add(new CustomSendingInventoryDataCell(ItemsNetData.Value.Cells[i]));
            }

            foreach (var cell in item.Cells)
            {
                var rand = Random.Range(cell.ItemsRange.x, cell.ItemsRange.y);
                var desiredCellId =
                    InventoryHelper.GetDesiredCellId(cell.ResultItem.Id, rand, ItemsNetData, new Vector2Int(5, 10));
                if (desiredCellId == -1)
                {
                    _recycling = false;
                    Debug.LogError("Recycler: Can't find item placed in desired cell");
                    return;
                }

                SetItem(desiredCellId, new CustomSendingInventoryDataCell(cell.ResultItem.Id, rand, -1, 0));
            }

            RemoveItemCountFromSlot(slotIndex, item.Id, 1);
            _recycling = false;
        }

        private void TryRecycle()
        {
            if (!IsServer) return;
            if (_recycling) return;

            for (int i = 0; i < ItemsNetData.Value.Cells.Length; i++)
            {
                var cell = ItemsNetData.Value.Cells[i];
                var item = ItemFinder.singleton.GetItemById(cell.Id);
                if (cell.Id == -1 || !(item is RecyclingItem)) continue;
                RecycleItem(item as RecyclingItem, i);
                _recycling = true;
                return;
            }

            SetTurnedServerRpc(false);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetTurnedServerRpc(bool value)
        {
            if (!IsServer) return;
            Turned.Value = value;
        }
    }
}
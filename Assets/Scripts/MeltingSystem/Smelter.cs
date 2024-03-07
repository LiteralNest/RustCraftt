using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory_System;
using Inventory_System.Slots_Displayer;
using Items_System.Items;
using Items_System.Items.Abstract;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace MeltingSystem
{
    public class Smelter : DropableStorage
    {
        public NetworkVariable<bool> Flaming { get; private set; } = new(false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Main Params")] [SerializeField]
        private GameObject _fireObject;

        [Header("Range")] [SerializeField] protected Vector2Int _fuelSlotsRange;
        [SerializeField] protected Vector2Int _inputSlotsRange;
        [SerializeField] protected Vector2Int _outputSlotsRange;

        [Header("Sound")] [SerializeField] private AudioSource _source;

        [Header("UI")] [SerializeField] private GameObject _turnOnPanel;
        [SerializeField] private GameObject _turnOffPanel;

        [Header("Drop Bag Staff")] [SerializeField]
        private StorageSlotsDisplayer _bagSlotsDisplayer;

        [SerializeField] private GameObject _targetUI;

        private void Start()
            => gameObject.tag = "CampFire";

        private void Update()
            => Cook();

        protected bool IsInRange(int value, Vector2 range)
            => value >= range.x && value < range.y;

        public override void OnNetworkSpawn()
        {
            Flaming.OnValueChanged += (bool prevValue, bool newValue) => { TurnFire(newValue); };
            TurnFire(Flaming.Value);
            base.OnNetworkSpawn();
            if (WasDropped.Value)
                DisplayBag();

            WasDropped.OnValueChanged += (_, _) =>
            {
                if (WasDropped.Value)
                    DisplayBag();
            };
        }

        private void DisplayBag()
        {
            Ui = _targetUI;
            SlotsDisplayer = _bagSlotsDisplayer;
            if (IsServer)
                Turn(false);
        }

        public override int GetAvailableCellIndexForMovingItem(Item item)
        {
            var cells = ItemsNetData.Value.Cells.ToList();
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].Id != -1) continue;
                if (CanAddItem(item, i)) return i;
            }

            return -1;
        }

        [ServerRpc(RequireOwnership = false)]
        public void TurnFlamingServerRpc(bool value)
            => Turn(value);

        private void Turn(bool value)
        {
            List<Fuel> items = new List<Fuel>();
            if (value)
            {
                items = GetFuel();
                if (items.Count == 0) return;
                Flaming.Value = true;
            }
            else
            {
                _source.Stop();
                Flaming.Value = false;
                return;
            }

            _source.Play();
            StartCoroutine(RemoveFuel(items[0]));
        }

        private void TurnFire(bool value)
        {
            _turnOnPanel.SetActive(!value);
            _turnOffPanel.SetActive(value);
            _fireObject.SetActive(value);
        }

        private List<Fuel> GetFuel()
        {
            List<Fuel> res = new List<Fuel>();
            var cells = ItemsNetData.Value.Cells;
            for (int i = _fuelSlotsRange.x; i < _fuelSlotsRange.y; i++)
            {
                if (cells[i].Id == -1) continue;
                var item = ItemFinder.singleton.GetItemById(cells[i].Id);
                if (item is Fuel && cells[i].Count > 0)
                    res.Add(item as Fuel);
            }

            return res;
        }

        private IEnumerator RemoveFuel(Fuel fuel)
        {
            var cells = ItemsNetData.Value.Cells;
            RemoveItemCountServerRpc(fuel.Id, 1);
            SlotsDisplayer.DisplayCells();

            yield return new WaitForSeconds(fuel.BurningTime);
            if (Flaming.Value)
            {
                var fuelList = GetFuel();
                if (fuelList.Count != 0)
                    StartCoroutine(RemoveFuel(fuelList[0]));
                else
                    Turn(false);
            }
        }

        protected virtual void Cook()
        {
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Inventory_System;
using Items_System.Items;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace MeltingSystem
{
    public class Smelter : Storage
    {
        public NetworkVariable<bool> Flaming { get; private set; } = new(false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Main Params")] [SerializeField]
        private GameObject _fireObject;

        [Header("Range")] 
        [SerializeField] protected Vector2Int _fuelSlotsRange;
        [SerializeField] protected Vector2Int _inputSlotsRange;
        [SerializeField] protected Vector2Int _outputSlotsRange;

        [Header("Sound")] [SerializeField] private AudioSource _source;
         
        private void Start()
            => gameObject.tag = "CampFire";

        protected bool IsInRange(int value, Vector2 range)
            => value >= range.x && value < range.y;

        public override void OnNetworkSpawn()
        {
            Flaming.OnValueChanged += (bool prevValue, bool newValue) => { TurnFire(); };
            TurnFire();
            base.OnNetworkSpawn();
        }

        private void Update()
        {
            Cook();
        }

        private void TurnFire()
            => _fireObject.SetActive(Flaming.Value);

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
                    TurnFlamingServerRpc(false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void TurnFlamingServerRpc(bool value)
        {
            List<Fuel> items = new List<Fuel>();
            Flaming.Value = value;
            if (value)
            {
                items = GetFuel();
                if (items.Count == 0) return;
            }
            else
            {
                _source.Stop();
                return;
            }

            _source.Play();
            StartCoroutine(RemoveFuel(items[0]));
        }

        protected virtual void Cook()
        {

        }
    }
}
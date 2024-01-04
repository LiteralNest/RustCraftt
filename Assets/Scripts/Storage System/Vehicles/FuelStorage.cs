using System.Collections.Generic;
using System.Collections;
using Items_System.Items.Abstract;
using UnityEngine;

namespace Storage_System.Vehicles
{
    public class FuelStorage : Storage
    {
        [SerializeField] private List<Item> _availableItems;
        [SerializeField] private float _timeBetweenRemovingFuel = 1;
        private Coroutine _removeFuelRoutine;

        private void Update()
        {
        }

        private IEnumerator RemoveFuelRoutine()
        {
            while (GetAvailableFuel() > 0)
            {
                yield return new WaitForSeconds(_timeBetweenRemovingFuel);
                RemoveItemCountServerRpc(_availableItems[0].Id, 1);
            }
        }

        public override bool CanAddItem(Item item, int index)
        {
            if (!_availableItems.Contains(item)) return false;
            return true;
        }

        private int GetAvailableFuel()
        {
            int res = 0;
            foreach (var cell in ItemsNetData.Value.Cells)
                res += cell.Count;
            return res;
        }

        public void StartMoving()
            => _removeFuelRoutine = StartCoroutine(RemoveFuelRoutine());

        public void StopMoving()
        {
            if (_removeFuelRoutine == null) return;
            StopCoroutine(_removeFuelRoutine);
        }

        public bool FuelAvailable()
            => GetAvailableFuel() > 0;
    }
}
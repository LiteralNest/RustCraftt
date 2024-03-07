using Inventory_System;
using Storage_System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CorpSystem
{
    public class PlayerCorpDecay : BaseCorpDecay
    {
        [Header("Player Rotting")] [SerializeField]
        private Storage _playerStorage;

        [SerializeField] private int _emptyDecaySecondsTime = 60;

        private Coroutine _rottingCoroutine;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsServer) return;
            StartDecay();
            _playerStorage.ItemsNetData.OnValueChanged += ((value, newValue) => StartDecay());
        }

        private void StartDecay()
        {
            if (_rottingCoroutine != null)
                StopCoroutine(_rottingCoroutine);
            _rottingCoroutine = StartCoroutine(StartDecayRoutine(GetDecaySeconds()));
        }

        private int GetDecaySeconds()
        {
            foreach (var slot in _playerStorage.ItemsNetData.Value.Cells)
            {
                if (slot.Id == -1) continue;
                return (int)ItemFinder.singleton.GetItemById(slot.Id).DestroySecondsTime;
            }

            return _emptyDecaySecondsTime;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Items_System.Ore_Type
{
    public class Ore : NetworkBehaviour
    {
        [Header("Start init")] [SerializeField]
        protected int _hp = 100;

        [SerializeField] protected NetworkVariable<int> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private float _recoveringSpeed;
        [SerializeField] protected List<OreSlot> _resourceSlots = new List<OreSlot>();
        [SerializeField] private List<GameObject> _renderers;
        [SerializeField] private GameObject _vfxEffect;

        public bool Recovering { get; protected set; } = false;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                _currentHp.Value = _hp;

            _currentHp.OnValueChanged += (int prevValue, int newValue) => { CheckHp(); };

            base.OnNetworkSpawn();
        }

        protected void AddResourcesToInventory()
        {
            foreach (var slot in _resourceSlots)
            {
                var rand = Random.Range(slot.CountRange.x, slot.CountRange.y + 1);
                GlobalEventsContainer.OnInventoryItemAdded?.Invoke(new InventoryCell(slot.Resource, rand));
                InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(slot.Resource.Id, rand, 0);
            }
        }

        private void CheckHp()
        {
            if (_currentHp.Value > 0) return;
            DestroyObject();
        }

        protected void TurnRenderers(bool value)
        {
            foreach (var renderer in _renderers)
                renderer.SetActive(value);
        }

        private IEnumerator Recover()
        {
            Recovering = true;
            yield return new WaitForSeconds(_recoveringSpeed);
            Recovering = false;
            TurnRenderers(true);
        }

        protected virtual void DestroyObject()
            => StartCoroutine(Destroy());

        private IEnumerator Destroy()
        {
            TurnRenderers(false);
            yield return Recover();
            if (NetworkManager.Singleton.IsServer)
                _currentHp.Value = _hp;
        }

        [ServerRpc(RequireOwnership = false)]
        protected void MinusHpServerRpc()
        {
            if (_currentHp.Value <= 0) return;
            _currentHp.Value--;
        }

        [ClientRpc]
        private void DisplayVfxClientRpc(Vector3 pos, Vector3 rot)
        {
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
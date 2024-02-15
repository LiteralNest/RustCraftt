using System.Collections.Generic;
using CloudStorageSystem;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public class DropableStorage : Storage
    {
        [Header("Dropable Storage")] [SerializeField]
        private List<Renderer> _targetRenders;

        [SerializeField] private Transform _fatherTransform;
        [SerializeField] private List<Collider> _colliders;
        [SerializeField] private List<GameObject> _disalingObjects;
        [SerializeField] private GameObject _bag;

        protected NetworkVariable<bool> WasDropped = new(false);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (WasDropped.Value) DisplayBag();

            if (IsServer)
            {
                ItemsNetData.OnValueChanged += (_, _) =>
                {
                    CloudSaveEventsContainer.OnStructureInventoryChanged?.Invoke(_fatherTransform.position,
                        ItemsNetData.Value);
                };
            }

            WasDropped.OnValueChanged += (_, _) =>
            {
                if (WasDropped.Value) DisplayBag();
            };
        }

        private bool StorageFree()
        {
            foreach (var item in ItemsNetData.Value.Cells)
                if (item.Id != -1)
                    return false;
            return true;
        }

        private void DisplayBag()
        {
            foreach (var render in _targetRenders)
                render.enabled = false;
            foreach (var collider in _colliders)
                collider.enabled = false;
            foreach (var obj in _disalingObjects)
                obj.SetActive(false);
            _bag.gameObject.SetActive(true);
        }

        public bool TryDisplayBagOnServer()
        {
            if (StorageFree()) return false;
            WasDropped.Value = true;
            return true;
        }
    }
}